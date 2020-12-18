using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("TimberMan/Player/Player")]
public class Player : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] float startingTimer = 5;
    [SerializeField] int numberOfChopToDecrease = 20;
    [SerializeField] float decreaseValue = 0.25f;
    [SerializeField] float minimumValue = 0.5f;

    public System.Action<bool> onTap;
    public System.Action<bool> onDie;
    public System.Action<float> onUpdateTimer;

    bool dead;

    float timerMax;
    float timer;

    int choppedTrunks;
    int previousLimitChop;

    private void Start()
    {
        //set start timer
        timerMax = startingTimer;
        timer = timerMax;
    }

    void Update()
    {
        //update timer
        timer -= Time.deltaTime;
        onUpdateTimer?.Invoke(timer / timerMax);
    }

    #region private API

    void UpdateChoppedTrunks()
    {
        //increase chopped trunks and update UI
        choppedTrunks++;
        GameManager.instance.uiManager.UpdateChoppedTrunks(choppedTrunks);

        //if reached number, decrease timer max
        if((previousLimitChop + numberOfChopToDecrease) % choppedTrunks == 0)
        {
            timerMax = Mathf.Max(minimumValue, timerMax - decreaseValue);   //clamp to minimum
            previousLimitChop = choppedTrunks;
        }

        //reset timer
        timer = timerMax;
    }

    void Die(bool killedByBranch)
    {
        //be sure player is not already dead
        if (dead)
            return;

        //set player dead
        dead = true;

        onDie?.Invoke(killedByBranch);
    }

    #endregion

    /// <summary>
    /// Called by canvas
    /// </summary>
    /// <param name="rightTap">tap left or right button?</param>
    public void OnTap(bool rightTap)
    {
        //be sure player is not dead
        if (dead)
            return;

        onTap?.Invoke(rightTap);

        //increase chopped trunks
        UpdateChoppedTrunks();

        //check if player is died after chop
        if (GameManager.instance.levelManager.PlayerDieOnChop(rightTap))
        {
            Die(true);
        }
    }
}
