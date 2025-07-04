using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



//this script is used to bridge objects together allowing them to communicate with each other
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private PlayerDataManager characterPrefab; //a value of the character prefab to spawn into the scene 
    [SerializeField] private AIPlayerController npcPrefab;
    [SerializeField] private int StartingNpcCount;
    private PlayerDataManager _playerInstance;
    private  List<AIPlayerController> _npcInstances;

    //for spawn points
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private List<Transform> enemySpawnPoints;





    public void SpawnCharacter()
    {
  
        
        _npcInstances = new List<AIPlayerController>();

        //makes player spawn in spawn point
        if (playerSpawnPoint != null)
        {
            _playerInstance = Instantiate(characterPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Player spawn point is not assigned!");
        }

        for (int i = 0; i < StartingNpcCount; ++i)
        {
            if (enemySpawnPoints.Count > 0)
            {
                Transform spawnPoint = enemySpawnPoints[i % enemySpawnPoints.Count]; // loop through available spawns
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

   


    void Start()
    {
        if (_playerInstance != null)
        {
            _playerInstance.OnDeath += HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("HandlePlayerDeath() called!");
        // Disable movement, show death screen, etc.
        Destroy(_playerInstance.gameObject); // Optional: destroy the whole player
    }


    private void OnBaddieKilled()
    {
        Debug.Log("BaddieKilled");
        _playerInstance.TellPlayerBaddieDied();
    }
}
