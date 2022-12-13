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
    public TestPlayer player;
    public string currentSceneName;
    public Cinemachine.CinemachineConfiner cameraBorder;
    private Scene currenScene;
    private PlayableDirector director;

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
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //if(!Debugging)
        //    SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        InputHandler.instance.SetActionEnable(gameState);
        if(gameState == GameState.Prologue)
        {
            InputHandler.instance.OnUIConfirm += (ctx) => ctx = new InputHandler.InputArgs();
            InputHandler.instance.OnUIBack += (ctx) => ctx = new InputHandler.InputArgs();
        }
        player = FindObjectOfType<TestPlayer>();
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Scene Management

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene == SceneManager.GetSceneByName("BasicLevel"))
        {
            for(int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                LoadScene(i);                
            }
            DoForSeconds(() => player.transform.position = GetGameObjectInScene("CH1", "StartPoint").transform.position, Time.deltaTime);
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
        this.gameState = state;
        InputHandler.instance.SetActionEnable(gameState);
    }
    public void ChangeCam(PlayableAsset play)
    {
        director.playableAsset = play;
        director.Play();
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
