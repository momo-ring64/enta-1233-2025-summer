using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : MonoBehaviour
{

    [Header("Attack Stats")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int attackDamage = 1;

    private float _attackTimer = 0f;
    private Transform _playerTransform;
    private PlayerDataManager _playerData;

    [Header("Health")]
    [SerializeField] private int MaxHp;
    [SerializeField] private HealthBarDisplay HealthDisplay;
    [SerializeField] private int DamageLayer = 8;

    [Header("Death Audio")]
    [SerializeField] private AudioSource DeathSourcePrefab;
    [SerializeField] private AudioClip DeathSound;

    [Header("Blood")]
    [SerializeField] private GameObject BloodParticlePrefab;

    [SerializeField] private GameObject DeathEffectPrefab;




    private PlayerDataManager playerDataManager;



    public Action OnDeath;
    private int _currentHp;

    // Start is called before the first frame update
 

    void Start()
    {
        _playerTransform = PlayerLocatorSingleton.Instance?.transform;
        playerDataManager = PlayerLocatorSingleton.Instance?.GetComponentInParent<PlayerDataManager>();

        /*
        Debug.Log("Assigned player transform: " + _playerTransform?.name);
        Debug.Log("Found player data: " + _playerData?.name);
        Debug.Log("Assigned playerDataManager: " + playerDataManager);
        */


        _currentHp = MaxHp;

        if (PlayerLocatorSingleton.Instance != null)
        {
            _playerData = PlayerLocatorSingleton.Instance.GetComponentInParent<PlayerDataManager>();

            if (_playerData == null)
            {
                Debug.LogWarning("playerdatamanager not found in parent of playerlocatorsingleton!");
            }
        }
        else
        {
            Debug.LogWarning("playerlocatorsingleton.nstance is null!");
        }
    }






    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == DamageLayer)
        {
            Debug.Log("PEW!");
            _currentHp--;

            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;

            if (BloodParticlePrefab != null)
            {
                GameObject bloodFx = Instantiate(BloodParticlePrefab, hitPoint, Quaternion.LookRotation(contact.normal));
                Destroy(bloodFx, 0.5f); // Clean up after a few seconds
            }

            OnDamagetaken();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_playerTransform != null && playerDataManager != null)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);


            if (distance <= attackRange && _attackTimer >= attackCooldown)
            {
                Debug.Log("ai is in range and ready to attack");
                AttackPlayer();
                _attackTimer = 1f;
            }
        }
        else
        {
            Debug.LogWarning("ai missing transform or playeraatamanager");
        }
    }





    private void AttackPlayer()
    {
        if (playerDataManager != null)
        {
            Debug.Log("AI attacks player!");
            playerDataManager.TakeDamage(attackDamage);
        }
        else
        {
            Debug.LogWarning("PlayerDataManager reference not set!");
        }
    }



    private void OnDamagetaken()
    {
        float currentHpPercent = (float)_currentHp / MaxHp;
        HealthDisplay.UpdateHp(currentHpPercent);

        if (_currentHp <= 0)
        {
            // Spawn audio source at death location
            AudioSource tempAudio = Instantiate(DeathSourcePrefab, transform.position, Quaternion.identity);
            tempAudio.PlayOneShot(DeathSound);

            //spawn smoke effect
            if (DeathEffectPrefab != null)
            {
                GameObject smoke = Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);
                Destroy(smoke, 3f); // optional: destroy the smoke after it's done
            }

            // Clean up audio source after clip duration
            Destroy(tempAudio.gameObject, DeathSound.length);

            // Tell player they got a kill and destroy enemy
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }


}
