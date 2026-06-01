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
    
    [SerializeField]
    private GameObject _retryCanva;

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
            if (_pauseCanva == null)
            {
                return;
            }

            if (_gameState == GameState.Menu)       // Cannot pause or resume the game when in the menu
            {
                return;
            }

            if (_gameState == GameState.Play)       // pause the game
            {
                Time.timeScale = 0f;
                _pauseCanva.gameObject.SetActive(true);
                _gameState = GameState.Pause;
                return;
            }

            if (_gameState == GameState.Pause)      // resume the game
            {
                OnResumeTriggered();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))            // This is just for testing purposes, you can remove this when you have an event that triggers the retry canvas when the player dies
        {
            OnActivateRetryCanvas();
        }
    }

    private void OnActivateRetryCanvas()            // This method can be called from an event when the player dies to show the retry canvas
    {
        if (_retryCanva == null)
        {
            return;
        }

        Time.timeScale = 0f;
        _retryCanva.gameObject.SetActive(true);
        _gameState = GameState.Pause;

    }

    public void OnReloadSceneTriggered()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;                        // Ensure the game is running when reloading the scene
        _retryCanva.gameObject.SetActive(false);    // Ensure the pause canvas is hidden when reloading the scene
        _gameState = GameState.Play;
    }
    public void OnResumeTriggered()
    {
        Time.timeScale = 1f;
        _pauseCanva.gameObject.SetActive(false);
        _gameState = GameState.Play;
    }


    public void OnChangeSceneTriggered(int sceneIndex)
    {
        if (sceneIndex == 0)                        //0 is the index of the menu scene, so we set the game state to Menu when loading it otherwise we set it to Play
        {
            _gameState = GameState.Menu;
        }
        else
        {
            _gameState = GameState.Play;
        }

        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f;                        // Ensure the game is running when changing scenes
        _pauseCanva.gameObject.SetActive(false);    // Ensure the pause canvas is hidden when changing scenes
        _retryCanva.gameObject.SetActive(false);    // Ensure the retry canvas is hidden when changing scenes
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
