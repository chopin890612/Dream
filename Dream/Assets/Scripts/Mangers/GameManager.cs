using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private bool Debugging;
    public static GameManager instance;
    public GameState gameState;
    public GameState previousState;
    public TestPlayer player;
    public string currentSceneName;
    private Scene currenScene;

    public GameObject PauseUI;

    public InputAction action;

    public enum GameState
    {
        Prologue,
        MainMenu,
        GameView,
        GameMenu,
        Dialogue
    }
    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        if (!Debugging)
            SceneManager.sceneLoaded += OnSceneLoaded;

        action.Enable();
        action.performed += ctx => SceneManager.LoadScene(0);
    }
    void Start()
    {
        InputHandler.instance.SetActionEnable(gameState);
        
        
        //if (gameState == GameState.Prologue)
        //{
        //    InputHandler.instance.OnUIConfirm += (ctx) => ctx = new InputHandler.InputArgs();
        //    InputHandler.instance.OnUIBack += (ctx) => ctx = new InputHandler.InputArgs();
        //}
        //player = FindObjectOfType<TestPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Scene Management

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PauseUI.SetActive(false);
        switch (scene.buildIndex)
        {
            case 0:
                ChangeGameState(GameState.MainMenu);
                break;
            case 1:
                ChangeGameState(GameState.Prologue);
                break;
            case 2:
                ChangeGameState(GameState.GameView);
                player = FindObjectOfType<TestPlayer>();
                InputHandler.instance.OnPause += arg => OnPause();
                InputHandler.instance.OnUIPause += arg => OnPause();
                break;
        }
    }
    public void LoadScene(int sceneIndex)
    {
        //SceneManager.UnloadSceneAsync(currenScene);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }

    #endregion

    public void ChangeGameState(GameState state)
    {
        previousState = gameState;
        gameState = state;
        InputHandler.instance.SetActionEnable(gameState);
    }
    public void OnPause()
    {
        if (!PauseUI.activeSelf)
        {
            PauseUI.SetActive(true);
            ChangeGameState(GameState.GameMenu);
        }
        else
        {
            PauseUI.SetActive(false);
            ChangeGameState(previousState);
        }
    }







    public void DoForSeconds(System.Action action ,float seconds)
    {
        StartCoroutine(WaitCoroutin(action, seconds));
    }
    private IEnumerator WaitCoroutin(System.Action action, float secconds)
    {
        yield return new WaitForSeconds(secconds);
        action();
    }
    public void SetControllerVibration(float LMotor, float RMotor, float seconds)
    {
        GamePad.SetVibration(playerIndex, LMotor, RMotor);
        DoForSeconds(() => GamePad.SetVibration(playerIndex, 0, 0), seconds);
    }

    /// <summary>
    /// Find Gameobject in Specific Loaded Scene.(In Scene ROOT only)
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public GameObject GetGameObjectInScene(string sceneName, string objectName)
    {
        foreach(GameObject ob in SceneManager.GetSceneByName(sceneName).GetRootGameObjects())
        {
            if(ob.name == objectName) return ob;
        }
        Debug.LogWarning($"Can't NOT find \"{objectName}\" in \"{sceneName}\".");
        return null;
    }
}
