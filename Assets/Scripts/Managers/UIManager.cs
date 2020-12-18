namespace redd096
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("redd096/MonoBehaviours/UI Manager")]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu = default;
        [SerializeField] Text choppedTrunks = default;

        void Start()
        {
            PauseMenu(false);
        }

        public void PauseMenu(bool active)
        {
            if (pauseMenu == null)
                return;

            //active or deactive pause menu
            pauseMenu.SetActive(active);
        }

        public void UpdateChoppedTrunks(int number)
        {
            if (choppedTrunks == null)
                return;

            //update text
            choppedTrunks.text = number.ToString();
        }
    }
}