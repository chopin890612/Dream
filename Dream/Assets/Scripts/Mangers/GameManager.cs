using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    public InputAction action;

    public enum GameState
    {
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
        if(!Debugging)
            SceneManager.sceneLoaded += OnSceneLoaded;

        action.Enable();
        action.performed += LoadScene;
    }
    void Start()
    {
        InputHandler.instance.SetActionEnable(gameState);
        player = FindObjectOfType<TestPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene != SceneManager.GetSceneByBuildIndex(0))
        {
            currentSceneName = scene.name.ToString();
            currenScene = scene;
            Transform startPoint = GameObject.Find("StartPoint").transform;
            player.transform.position = startPoint.position;
            cameraBorder.m_BoundingVolume = GameObject.Find("CameraBorder").GetComponent<Collider>();
        }
        else if(scene == SceneManager.GetSceneByBuildIndex(0))
        {
            if(SceneManager.sceneCount == 1)
                SceneManager.LoadScene(5, LoadSceneMode.Additive);
        }
    }
    private void LoadScene(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(5, LoadSceneMode.Additive);
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(currenScene);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }
    public void ChangeGameState(GameState state)
    {
        this.gameState = state;
        InputHandler.instance.SetActionEnable(gameState);
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
}
