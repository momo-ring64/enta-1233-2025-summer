using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//this script is used to store the data of the player into the ui to give accurate numbers 
public class PlayerHUD : HealthBarDisplay
{

    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ScoreText;

    private void Start()
    {
        OnScoreUpdated(0);
    }
    public void OnHealthUpdated(int currentHealth, int maxHealth)
    {
        HealthText.text = $"{currentHealth}/{maxHealth}";

        // Update the red health bar fill
        float percent = (float)currentHealth / maxHealth;
        UpdateHp(percent);
    }


    public void OnScoreUpdated(int score)
    {
        ScoreText.text = $"{score} baddies killed!";
    }

}
