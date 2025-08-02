using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//this script is used to store the data of the player into the ui to give accurate numbers 
public class PlayerHUD : HealthBarDisplay
{

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    

    private void Start()
    {
        OnScoreUpdated(0);
    }
    public void OnHealthUpdated(int currentHealth, int maxHealth)
    {
        healthText.text = $"{currentHealth}/{maxHealth}";

        // Update the red health bar fill
        float percent = (float)currentHealth / maxHealth;
        UpdateHp(percent);
    }
    

    public void OnScoreUpdated(int score)
    {
        scoreText.text = $"{score} baddies killed!";
    }

    public void TimeUpdated(string time)
    {
        timerText.text = $"Time: {time}";
    }


}
