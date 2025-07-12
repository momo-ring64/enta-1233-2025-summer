using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//this script is used to bridge objects together allowi ng them to communicate with each other
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private PlayerDataManager characterPrefab; //a value of the character prefab to spawn into the scene 
    [SerializeField] private AIPlayerController npcPrefab;
    [SerializeField] private int StartingNpcCount;
    private PlayerDataManager _playerInstance;
    private  List<AIPlayerController> _npcInstances;

    //for spawn points
    [Header("Spawn Points")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> enemySpawnPoints;

    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private Camera deathCamera;
    






    public void SpawnCharacter()
    {
        _npcInstances = new List<AIPlayerController>();

        // Spawn player
        if (playerSpawnPoint != null)
        {
            _playerInstance = Instantiate(characterPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

            // Hook into player death *after* spawning
            _playerInstance.OnDeath += HandlePlayerDeath;
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned!");
        }

        // Spawn NPCs
        for (int i = 0; i < StartingNpcCount; ++i)
        {
            if (enemySpawnPoints.Count > 0)
            {
                Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Count];
                AIPlayerController spawnedNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
                spawnedNpc.OnDeath = OnBaddieKilled;
                _npcInstances.Add(spawnedNpc);
            }
            else
            {
                Debug.LogWarning("No enemy spawn points assigned!");
            }
        }
    }






    private void HandlePlayerDeath()
    {
        Debug.Log("HandlePlayerDeath() called!");

      
        Camera playerCam = _playerInstance.GetComponentInChildren<Camera>();
        if (playerCam != null && deathCamera != null)
        {
           
            deathCamera.transform.position = playerCam.transform.position;
            deathCamera.transform.rotation = playerCam.transform.rotation;

          
            deathCamera.enabled = true;

        
            AudioListener playerAudio = playerCam.GetComponent<AudioListener>();
            if (playerAudio != null) playerAudio.enabled = false;
        }

        // Destroy player object
        Destroy(_playerInstance.gameObject);
    }



    private void OnBaddieKilled()
    {
        Debug.Log("BaddieKilled");
        _playerInstance.TellPlayerBaddieDied();
    }
}
