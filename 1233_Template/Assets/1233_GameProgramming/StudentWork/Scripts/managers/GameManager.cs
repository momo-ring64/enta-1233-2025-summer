using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance { get; private set; }

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }
    private void Start()
    {
        InitializeGame();
    }
    public void InitializeGame()
    {
        levelManager.LoadLevel("SimpleLevel");

    }
    public void RestartGame()
    {
        levelManager.LoadLevel("SimpleLevel");

       

    }
    

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
