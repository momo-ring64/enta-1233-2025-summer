using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    public int Health;
    public TextMeshProUGUI HealthText;

    public void SpawnCharacter()
    {
        Vector3 spawnPosition = Vector3.zero;
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity, transform);
    }

    //create a new input to aim 

    // Start is called before the first frame update
    void Start()
    {
        HealthText.text = "Health: " + Health.ToString();
        
        //store health value 
    }
}
