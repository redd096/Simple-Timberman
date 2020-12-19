namespace redd096
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    [AddComponentMenu("redd096/MonoBehaviours/UI Manager")]
    public class UIManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject startMenu = default;
        [SerializeField] GameObject endMenu = default;

        [Header("Important")]
        [SerializeField] Slider timerSlider = default;
        [SerializeField] Text choppedText = default;

        [Header("Level Up")]
        [SerializeField] Text levelText = default;
        [SerializeField] string levelString = "Level ";
        [SerializeField] float durationLevelUp = 1;

        Coroutine deactiveUpdateLevelCoroutine;

        void Start()
        {
            //be sure to deactivate
            endMenu.SetActive(false);
            levelText.gameObject.SetActive(false);
        }

        public void StartMenu(bool active)
        {
            //start menu
            startMenu.SetActive(active);
        }

        public void EndMenu(bool active)
        {
            //end menu
            endMenu.SetActive(active);
        }

        public void UpdateTimer(float value)
        {
            //update slider
            timerSlider.value = value;
        }

        public void UpdateChoppedTrunks(int number)
        {
            //update text
            choppedText.text = number.ToString();
        }

        public void UpdateLevel(int level)
        {
            //set text and active
            levelText.text = levelString + level.ToString();
            levelText.gameObject.SetActive(true);

            //start coroutine to deactive
            if (deactiveUpdateLevelCoroutine != null)
                StopCoroutine(deactiveUpdateLevelCoroutine);

            deactiveUpdateLevelCoroutine = StartCoroutine(DeactiveUpdateLevelCoroutine());
        }

        IEnumerator DeactiveUpdateLevelCoroutine()
        {
            //wait
            yield return new WaitForSeconds(durationLevelUp);

            //deactive
            levelText.gameObject.SetActive(false);
        }
    }
}