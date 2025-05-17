using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManaager : MonoBehaviour
{
    [SerializeField]
    public int Health;
    public TextMeshProUGUI HealthText;

    //create a new input to aim 

    // Start is called before the first frame update
    void Start()
    {
        HealthText.text = "Health: " + Health.ToString();
        
        //store health value 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
