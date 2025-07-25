using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//this script is used to bridge objects together allowi ng them to communicate with each other
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private PlayerDataManager characterPrefab; //a value of the character prefab to spawn into the scene 
    [SerializeField] private BaseAIController npcPrefab;
    [SerializeField] private int startingNpcCount;
    private PlayerDataManager _playerInstance;
    private  List<BaseAIController> _npcInstances;
    [SerializeField] private Camera gameEndCamera;

    //for spawn points
    [Header("Spawn Points")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> enemySpawnPoints;

    [Header("Player Game End Audio")]
    [SerializeField] private AudioSource deathSourcePrefab;
    [SerializeField] private AudioClip deathSound;

    [Header("Player Game Over Screen")]
    [SerializeField] private GameObject gameOverScreenCanvas;
    [SerializeField] private AudioSource gameOverSoundSource;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Player Victory Screen")]
    [SerializeField] private GameObject victoryScreenCanvas;
    [SerializeField] private AudioSource victorySoundSource;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private TMP_Text timerText;


    private int _aliveEnemies;





    public void SpawnCharacter()
    {
        _npcInstances = new List<BaseAIController>();

      
        if (playerSpawnPoint != null)
        {
            _playerInstance = Instantiate(characterPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

            
            _playerInstance.OnDeath += HandlePlayerDeath;
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned!");
        }

        _aliveEnemies = 0;

        for (int i = 0; i < startingNpcCount; ++i)
        {
            if (enemySpawnPoints.Count > 0)
            {
                Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Count];
                BaseAIController spawnedNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedNpc.OnDeath = OnBaddieKilled;
                _npcInstances.Add(spawnedNpc);
                _aliveEnemies++; 
            }
        }

    }



    private void Start()
    {
        gameEndCamera.gameObject.SetActive(false);
        SpawnCharacter();
    }

    private void ShowGameOverScreen()
    {
        Debug.Log("All enemies defeated!");
        Cursor.lockState = CursorLockMode.None;
        Destroy(_playerInstance.gameObject);

        if (gameOverScreenCanvas != null)
        {
            gameOverScreenCanvas.SetActive(true);
        }

        //func for turning on VictoryCanvas then unlocking the cursor
    }
    private void ShowVictoryScreen(string time)
    {
        Debug.Log("All enemies defeated!");
        Cursor.lockState = CursorLockMode.None;
        Destroy(_playerInstance.gameObject);

        if (victoryScreenCanvas != null)
        {
            victoryScreenCanvas.SetActive(true);
        }
        timerText.text = $"Time: {time}";
    }


    private void HandlePlayerDeath()
    {
        //death sound
        AudioSource tempAudio = Instantiate(deathSourcePrefab, transform.position, Quaternion.identity);
        tempAudio.PlayOneShot(deathSound);


        Debug.Log("HandlePlayerDeath() called!");
        
        Camera playerCam = _playerInstance.GetComponentInChildren<Camera>();
        if (playerCam != null && gameEndCamera != null)
        {
            gameEndCamera.transform.position = playerCam.transform.position;
            gameEndCamera.transform.rotation = playerCam.transform.rotation;
           
            gameEndCamera.gameObject.SetActive(true);
      
            AudioListener playerAudio = playerCam.GetComponent<AudioListener>();
            if (playerAudio != null) playerAudio.enabled = false;
        }
        ShowGameOverScreen();
        gameOverSoundSource.PlayOneShot(gameOverSound);
    }


    //func for turning on GameOverCanvas then unlocking the cursor
    


    private void OnBaddieKilled()
    {
        Debug.Log("BaddieKilled");
        _playerInstance.TellPlayerBaddieDied();

        _aliveEnemies--;

        if (_aliveEnemies <= 0)
        {
            Camera playerCam = _playerInstance.GetComponentInChildren<Camera>();
            if (playerCam != null && gameEndCamera != null)
            {
                gameEndCamera.transform.position = playerCam.transform.position;
                gameEndCamera.transform.rotation = playerCam.transform.rotation;

                gameEndCamera.gameObject.SetActive(true);

                AudioListener playerAudio = playerCam.GetComponent<AudioListener>();
                if (playerAudio != null) playerAudio.enabled = false;
            }
            //ShowVictoryScreen(); this is causing an error CS7036: There is no argument given that corresponds to the required parameter
            victorySoundSource.PlayOneShot(victorySound);
        }
    }


}
