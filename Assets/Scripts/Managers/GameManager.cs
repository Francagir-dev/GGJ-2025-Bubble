using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance => instance;
    private static GameManager instance;    
 

    [SerializeField] private LoadingBar oxygenBar;
    [SerializeField] private OxygenManager oxygenManager;

    public bool isSwimming;
    public bool isResting;
    public bool isDashing;
    public bool isInteract;
    public bool hasDead;
    /// <summary>
    /// Handles the game's data loading and saving
    /// </summary>
    private PersistenceManager persistenceManager;
    [SerializeField]private GameObject canvasDead;

    [Header("Objects to reset points")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shark;
    [Header("Initial positions")]
    [SerializeField] private Transform playerInit;
    [SerializeField] private Transform sharkInit;

    public GameObject[] doors = new GameObject[2];
    #region Initialization

    private void Awake()
    {
        if (!instance)
        {
            // Set the singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);   
        }
        else
        {
            // Set the references to the current scene's manager
            instance.oxygenBar = oxygenBar;
            if (instance.oxygenBar == null) 
                instance.oxygenBar = GameObject.Find(Constants.OXYGEN_HUD).GetComponent<LoadingBar>();
            instance.oxygenManager = oxygenManager;
            if (instance.oxygenManager == null) 
                instance.oxygenManager = GameObject.Find(Constants.OXYGEN_MANAGER).GetComponent<OxygenManager>();
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {       
        if (oxygenManager)
            oxygenManager.Init();
     
        if(persistenceManager)
            persistenceManager.Init();
              
    }

    #endregion




    #region Scene Management

    public void GoToMenu()
    {
        LoadScene(Constants.MAINMENU_NAME);
    }
    public void GoToGame()
    {
        LoadScene(Constants.MAINSCENE_NAME);
    }
    public void ExitGame()
    {
        Application.Quit(0);
    }
    private void LoadScene(string sceneName)
    {
      /*  if (loadingBar)
            StartCoroutine(LoadAsinchronouslyWithBar(sceneName));
        else*/
            SceneManager.LoadScene(sceneName);
    }
    private IEnumerator LoadAsinchronouslyWithBar(string sceneName)
    {
        //loadingBar.gameObject.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
           // loadingBar.SetProgress(progress);
            yield return null;
        }
        //yield return null;
    }

    #endregion

    #region Oxygen
    public void SetDefaultValuesToOxygenBar(int maxOxygen, int minOxygen,int actualOxygen) {
        oxygenBar.Maximum = maxOxygen;
        oxygenBar.Minimum = minOxygen;
        oxygenBar.Current = actualOxygen;
       }
    public void UpdateOxygenBar(int actualOxygen) {
        oxygenBar.Current = actualOxygen;
    }

    public void CanvasDead() {
        canvasDead.SetActive(true);
        Time.timeScale = 0f;
    }
    /// <summary>
    /// Change Decrease ratio for each speed when swimming
    /// </summary>
    /// <param name="newRatio">Ratio to decrease oxygen (from 1 to 10) </param>
    public void ChangeDecreaseRatio(int newRatio)
    {
        oxygenManager.oxygenRatio = Mathf.Clamp(newRatio, 1, 10);      

    }
    #endregion

    public void ResetPositions()
    {
        player.transform.position = playerInit.position;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        shark.transform.position = sharkInit.position;
        shark.transform.rotation = Quaternion.Euler(0, 0, 0);

        Time.timeScale = 1f;
    }
    public void CheckDoors() {
        
    }
}
