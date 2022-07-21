using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerIndex playerIndex = PlayerIndex.One;
    public static GameManager instance;
    public GameState gameState;

    public enum GameState
    {
        MainMenu,
        GameView,
        GameMenu
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        InputHandler.instance.SetActionEnable(gameState);
    }

    // Update is called once per frame
    void Update()
    {
        
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
