
using System.Collections;

using System.Diagnostics;

using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance => instance;
    private static GameManager instance;
    //



    public GameTimer timer;

    [SerializeField] private LoadingBar loadingBar;

    //

    public Stopwatch timeCount = new Stopwatch();

    /// <summary>
    /// Handles the game's data loading and saving
    /// </summary>
   // private PersistenceManager persistenceManager;

    #region Initialization

    private void Awake()
    {
        if (!instance)
        {
#if UNITY_EDITOR

#endif

            // Set the singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);
            timeCount = new Stopwatch();
        }
        else
        {
            // Set the references to the current scene's manager
            instance.loadingBar = loadingBar;



            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        /* if (instance.planeGameController)
         {
             instance.planeGameController.Init(
                 instance.currentMap, instance.GetSelectedPlane(),
                 instance.downloadedMaps.ContainsKey(instance.currentMap.Name)
             );
         }

         if (instance.mapController)
         {


         if (instance.timer)
             instance.timer.Init(backUpSpeed);

         // The user initial log in when the game starts
         if (instance.menuManager)
         {
             if (instance.ignoreLogin || instance.netManager.IsUserLogged)
                 instance.menuManager.SetPlayButtonScene(false);
             else
             {
                 instance.menuManager.SetPlayButtonScene(true);
                 instance.CheckCachedPlayerInfo();
             }
         }*/
    }

    #endregion




    #region Scene Management

    public void GoToMenu()
    {
        LoadScene("Constants.INITIALSCENENAME + Constants.MAINMENUSCEN");
    }

    private void LoadScene(string sceneName)
    {
        if (loadingBar)
            StartCoroutine(LoadAsinchronouslyWithBar(sceneName));
        else
            SceneManager.LoadScene(sceneName);
    }
    private IEnumerator LoadAsinchronouslyWithBar(string sceneName)
    {
        loadingBar.gameObject.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.SetProgress(progress);
            yield return null;
        }
        //yield return null;
    }

    #endregion
}
