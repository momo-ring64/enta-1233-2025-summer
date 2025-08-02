using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//this script is used to bridge objects together allowi ng them to communicate with each other
public class CharacterManager : MonoBehaviour
{
    private PlayerDataManager _playerInstance;
    private List<BaseAIController> _npcInstances;
    [SerializeField] private PlayerDataManager characterPrefab; //a value of the character prefab to spawn into the scene 
    [SerializeField] private BaseAIController npcPrefab;
    [SerializeField] private int startingNpcCount;
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
    [SerializeField] private TMP_Text deathTimerText;

    [Header("Player Victory Screen")]
    [SerializeField] private GameObject victoryScreenCanvas;
    [SerializeField] private AudioSource victorySoundSource;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private TMP_Text VictoryTimerText;


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

    private void ShowVictoryScreen(string time)
    {
        Debug.Log("Victory!");
        Cursor.lockState = CursorLockMode.None;
        Destroy(_playerInstance.gameObject);

        if (victoryScreenCanvas != null)
            victoryScreenCanvas.SetActive(true);

        if (VictoryTimerText != null)
            VictoryTimerText.text = $"Time: {time}";
    }

    private void ShowGameOverScreen(string time)
    {
        Debug.Log("Player died!");
        Cursor.lockState = CursorLockMode.None;
        Destroy(_playerInstance.gameObject);

        if (gameOverScreenCanvas != null)
            gameOverScreenCanvas.SetActive(true);

        if (deathTimerText != null)
            deathTimerText.text = $"Time: {time}";
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
        string time = _playerInstance.GetFormattedTime();
        ShowGameOverScreen(time);

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
            // get the formatted time from the player instance
            string levelTime = _playerInstance.GetFormattedTime();

            // setup camera when win game
            Camera playerCam = _playerInstance.GetComponentInChildren<Camera>();
            if (playerCam != null && gameEndCamera != null)
            {
                gameEndCamera.transform.position = playerCam.transform.position;
                gameEndCamera.transform.rotation = playerCam.transform.rotation;
                gameEndCamera.gameObject.SetActive(true);

                AudioListener playerAudio = playerCam.GetComponent<AudioListener>();
                if (playerAudio != null) playerAudio.enabled = false;
            }

            string time = _playerInstance.GetFormattedTime();
            ShowVictoryScreen(time);
            victorySoundSource.PlayOneShot(victorySound);
        }
    }



}
