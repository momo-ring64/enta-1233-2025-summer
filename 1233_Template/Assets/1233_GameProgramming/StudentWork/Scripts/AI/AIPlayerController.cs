using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerController : MonoBehaviour
{

    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int attackDamage = 1;

    private float _attackTimer = 0f;
    private Transform _playerTransform;
    private PlayerDataManager _playerData;

    [SerializeField] private int MaxHp;
    [SerializeField] private HealthBarDisplay HealthDisplay;
    [SerializeField] private int DamageLayer = 8;

    private PlayerDataManager playerDataManager;



    public Action OnDeath;
    private int _currentHp;

    // Start is called before the first frame update
 

    void Start()
    {
        _playerTransform = PlayerLocatorSingleton.Instance?.transform;
        Debug.Log("Assigned player transform: " + _playerTransform?.name);

        Debug.Log("Found player data: " + _playerData?.name);

        playerDataManager = PlayerLocatorSingleton.Instance?.GetComponentInParent<PlayerDataManager>();
        Debug.Log("Assigned playerDataManager: " + playerDataManager);


        _currentHp = MaxHp;

        if (PlayerLocatorSingleton.Instance != null)
        {
            _playerData = PlayerLocatorSingleton.Instance.GetComponentInParent<PlayerDataManager>();

            if (_playerData == null)
            {
                Debug.LogWarning("PlayerDataManager not found in parent of PlayerLocatorSingleton!");
            }
        }
        else
        {
            Debug.LogWarning("PlayerLocatorSingleton.Instance is null! Make sure it's active in the scene.");
        }
    }






    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == DamageLayer)
        {
            Debug.Log("PEW!");
            _currentHp--;
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
                Debug.Log("AI is in range and ready to attack");
                AttackPlayer();
                _attackTimer = 0f;
            }
        }
        else
        {
            Debug.LogWarning("AI missing transform or playerDataManager");
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

            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
