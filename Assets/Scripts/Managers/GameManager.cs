namespace redd096
{
    using UnityEngine;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uiManager { get; private set; }
        public LevelManager levelManager { get; private set; }
        public TreeManager treeManager { get; private set; }
        public Player player { get; private set; }

        int previousSceneIndex = -1;

        protected override void SetDefaults()
        {
            //get references
            uiManager = FindObjectOfType<UIManager>();
            levelManager = FindObjectOfType<LevelManager>();
            treeManager = FindObjectOfType<TreeManager>();
            player = FindObjectOfType<Player>();

            //if restarted same scene, and there is a level manager -> player clicked Restart, so start game immediatly
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == previousSceneIndex && levelManager != null)
            {
                levelManager.StartGame();
            }

            //save previous scene index
            previousSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }
    }
}