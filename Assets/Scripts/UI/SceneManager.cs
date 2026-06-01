using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public static ManagerScene Instance { get; private set; }

    private GameState _gameState;

    [SerializeField]
    private GameObject _pauseCanva;

    private void Awake()
    {
     
       

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            _gameState = GameState.Menu;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            print("Pausing");

            if (_pauseCanva == null)
            {
                return;
            }

            if (_gameState == GameState.Menu) // Cannot pause or resume the game when in the menu
            {
                return;
            }

            if (_gameState == GameState.Play) // pause the game
            {
                Time.timeScale = 0f;
                _pauseCanva.gameObject.SetActive(true);
                _gameState = GameState.Pause;
                return;
            }

            if (_gameState == GameState.Pause) // resume the game
            {
                OnResumeTriggered();
                return;
            }
        }
    }

    public void OnResumeTriggered()
    {
        Time.timeScale = 1f;
        _pauseCanva.gameObject.SetActive(false);
        _gameState = GameState.Play;
    }


    public void OnChangeSceneTriggered(int sceneIndex)
    {
        if (sceneIndex == 0) //0 is the index of the menu scene, so we set the game state to Menu when loading it otherwise we set it to Play
        {
            _gameState = GameState.Menu;
        }
        else
        {
            _gameState = GameState.Play;
        }

        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f; // Ensure the game is running when changing scenes
        _pauseCanva.gameObject.SetActive(false); // Ensure the pause canvas is hidden when changing scenes
    }

    public GameState GetGameState()
    {
        return _gameState;
    }
}

public enum GameState
{
    Play,
    Pause,
    Menu,
    Cinematic
}
