using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] private PlayerHUD PlayerHUD;
    [SerializeField] private int BaddieValue = 1;
    [SerializeField] private int maxHealth = 10;

    private int _playerscore = 0;
    private int currentHealth;

    public Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
        PlayerHUD.OnHealthUpdated(currentHealth, maxHealth);
        PlayerHUD.OnScoreUpdated(_playerscore);
    }

   

    private void Die()
    {
        Debug.Log("PLAYER DIED!");
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
        AddScore(BaddieValue);
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

   
}
