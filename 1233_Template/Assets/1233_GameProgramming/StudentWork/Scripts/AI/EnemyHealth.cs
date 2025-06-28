using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Visual Feedback")]
    public Renderer enemyRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Effects")]
    public GameObject hitEffect;
    public GameObject deathEffect;

    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        // Use the object's default renderer if not set manually
        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>();

        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Optional hit effect (e.g. particle)
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamage()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            enemyRenderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    // Call this from raycast weapons
    public void OnRaycastHit(int damage)
    {
        TakeDamage(damage);
    }

    // Handle physics projectiles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(25); // Default damage
        }
    }
}
