using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] private PlayerHUD PlayerHUD;
    [SerializeField] private int baddieValue = 1;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] TMP_Text timerText;
    [SerializeField] float remainingTime;

    private int _playerscore = 0;
    private int currentHealth;

    public Action OnDeath;
    public Action NoTime;

    void Start()
    {
        currentHealth = maxHealth;
        PlayerHUD.OnHealthUpdated(currentHealth, maxHealth);
        PlayerHUD.OnScoreUpdated(_playerscore);
    }


    //for the timer logic. i wanna do what we did for the ondeath action but for the timer so when it runs out of time, game ends. and when you complete it, it tells you your time.
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
        }

        
        
    }





    private void Die()
    {
        OnDeath?.Invoke(); // trigger anything listening to the death event
    }

    //add score when kill enemy plus maxes out health
    private void AddScore(int score)
    {
        _playerscore += score;
        PlayerHUD.OnScoreUpdated(_playerscore);

        // Heal to full when a baddie dies
        currentHealth = maxHealth;
        PlayerHUD.OnHealthUpdated(currentHealth, maxHealth);
    }

    public void TellPlayerBaddieDied()
    {
        AddScore(baddieValue);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player took damage! -" + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerHUD.OnHealthUpdated(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
            Die();
            // Optional: handle death (respawn, game over, etc.)
        }
    }

    public void RunOutTime()
    {
        if (remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
        }
    }
   
}
