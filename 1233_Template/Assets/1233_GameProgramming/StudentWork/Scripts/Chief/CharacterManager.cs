using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//this script is used to bridge objects together allowi ng them to communicate with each other
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private PlayerDataManager characterPrefab; //a value of the character prefab to spawn into the scene 
    [SerializeField] private BaseAIController npcPrefab;
    [SerializeField] private int StartingNpcCount;
    private PlayerDataManager _playerInstance;
    private  List<BaseAIController> _npcInstances;

    //for spawn points
    [Header("Spawn Points")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> enemySpawnPoints;

    [Header("Player Death Audio")]
    [SerializeField] private AudioSource DeathSourcePrefab;
    [SerializeField] private AudioClip DeathSound;

    [Header("Player Death Screen")]
    [SerializeField] private GameObject GameEndScreenUI;
    [SerializeField] private Camera GameEndCamera;


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

        for (int i = 0; i < StartingNpcCount; ++i)
        {
            if (enemySpawnPoints.Count > 0)
            {
                Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Count];
                BaseAIController spawnedNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedNpc.OnDeath = OnBaddieKilled;
                _npcInstances.Add(spawnedNpc);
                _aliveEnemies++; // Count it
            }
        }

    }






    private void HandlePlayerDeath()
    {

        AudioSource tempAudio = Instantiate(DeathSourcePrefab, transform.position, Quaternion.identity);
        tempAudio.PlayOneShot(DeathSound);


        Debug.Log("HandlePlayerDeath() called!");

        if (GameEndScreenUI != null)
        {
            GameEndScreenUI.SetActive(true);
        }
        

        // Get current player camera
        Camera playerCam = _playerInstance.GetComponentInChildren<Camera>();
        if (playerCam != null && GameEndCamera != null)
        {
            // Move death camera to player camera's position and rotation
            GameEndCamera.transform.position = playerCam.transform.position;
            GameEndCamera.transform.rotation = playerCam.transform.rotation;

            // Enable death camera
            GameEndCamera.gameObject.SetActive(true);


            // Optional: disable audio listener on player cam if needed
            AudioListener playerAudio = playerCam.GetComponent<AudioListener>();
            if (playerAudio != null) playerAudio.enabled = false;
        }

     


        Destroy(_playerInstance.gameObject);
    }

    private void ShowGameEndScreen()
    {
        Debug.Log("All enemies defeated!");

        if (GameEndScreenUI != null)
        {
            GameEndScreenUI.SetActive(true);
        }
    }


    private void OnBaddieKilled()
    {
        Debug.Log("BaddieKilled");
        _playerInstance.TellPlayerBaddieDied();

        _aliveEnemies--;

        if (_aliveEnemies <= 0)
        {
            ShowGameEndScreen();
        }
    }

}
