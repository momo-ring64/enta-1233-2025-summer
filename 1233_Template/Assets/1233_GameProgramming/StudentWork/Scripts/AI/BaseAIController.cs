using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAIController : MonoBehaviour
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
    [SerializeField] protected NavMeshAgent Agent;



    public Coroutine MoveCoroutine { get; private set; }
    protected bool _isDead = false;



    public Action OnDeath;
    private int _currentHp;

   

    void Start()
    {
        _playerTransform = PlayerLocatorSingleton.Instance?.transform;
        playerDataManager = PlayerLocatorSingleton.Instance?.GetComponentInParent<PlayerDataManager>();
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





    //keep
    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;
        if (collision.gameObject.layer == DamageLayer)
        {
            Debug.Log("PEW!");
            _currentHp--;

            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;

            if (BloodParticlePrefab != null)
            {
                GameObject bloodFx = Instantiate(BloodParticlePrefab, hitPoint, Quaternion.LookRotation(contact.normal));
                Destroy(bloodFx, 0.5f);
            }

            OnDamagetaken();
        }
    }



    private void Update()
    {
        if (Agent.enabled == false)
        {
            return;
        }
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
            Debug.LogWarning("ai missing transform or playerdatamanager");
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

    protected abstract void ReactToDamage();
 
    private void OnDamagetaken()
    {
        if (_isDead) return;

        // Disable all colliders to prevent future collisions
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            if (col.gameObject.CompareTag("Hitbox"))
                col.enabled = false;
        }


        // Optional: Stop knockback if it's still running
        if (this is AISwordController swordAI && swordAI.MoveCoroutine != null)
        {
            swordAI.StopCoroutine(swordAI.MoveCoroutine);
        }


        float currentHpPercent = (float)_currentHp / MaxHp;
        HealthDisplay.UpdateHp(currentHpPercent);

        if (_currentHp <= 0)
        {
            _isDead = true; 

            // death logic
            AudioSource tempAudio = Instantiate(DeathSourcePrefab, transform.position, Quaternion.identity);
            tempAudio.PlayOneShot(DeathSound);

            if (DeathEffectPrefab != null)
            {
                GameObject smoke = Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);
                Destroy(smoke, 3f);
            }

            Destroy(tempAudio.gameObject, DeathSound.length);

            OnDeath?.Invoke();
            Destroy(gameObject); 
        }
        else
        {
            ReactToDamage();
        }
    }
}
