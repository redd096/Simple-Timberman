using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("TimberMan/Managers/Level Manager")]
public class LevelManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] float startingTimer = 5;
    [SerializeField] int numberOfChopToDecrease = 20;
    [SerializeField] float decreaseValue = 0.25f;
    [SerializeField] float minimumValue = 0.5f;

    float timerMax;
    float timer;
    Coroutine timerCoroutine;

    int choppedTrunks;
    int previousLimitChop;

    int level;

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            //update timer and UI
            timer -= Time.deltaTime;
            GameManager.instance.uiManager.UpdateTimer(timer / timerMax);

            //if timer reach 0, player die
            if (timer <= 0)
            {
                GameManager.instance.player.Die(false);
                GameManager.instance.uiManager.EndMenu(true);
                break;
            }

            yield return null;
        }
    }

    void UpdateChoppedTrunks()
    {
        //increase chopped trunks and update UI
        choppedTrunks++;
        GameManager.instance.uiManager.UpdateChoppedTrunks(choppedTrunks);

        //if reached number, decrease timer max
        if (choppedTrunks >= (previousLimitChop + numberOfChopToDecrease))
        {
            timerMax = Mathf.Max(minimumValue, timerMax - decreaseValue);   //clamp to minimum
            previousLimitChop = choppedTrunks;

            level++;
            GameManager.instance.uiManager.UpdateLevel(level);
        }

        //reset timer
        timer = timerMax;
    }

    public void PlayerChop(bool rightTap)
    {
        //chop tree and update
        GameManager.instance.treeManager.PlayerChop();
        UpdateChoppedTrunks();

        //check if player die
        if (GameManager.instance.treeManager.PlayerDieOnChop(rightTap))
        {
            GameManager.instance.player.Die(true);
            GameManager.instance.uiManager.EndMenu(true);

            //stop timer coroutine
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
        }
    }

    /// <summary>
    /// Called from canvas (start menu)
    /// </summary>
    public void StartGame()
    {
        //remove start menu
        GameManager.instance.uiManager.StartMenu(false);

        //set start timer
        timerMax = startingTimer;
        timer = timerMax;

        //start timer coroutine
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerCoroutine());
    }
}
