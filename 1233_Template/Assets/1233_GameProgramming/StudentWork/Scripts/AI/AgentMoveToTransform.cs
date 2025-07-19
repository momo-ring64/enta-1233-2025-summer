using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToTransform : MonoBehaviour
{
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private float stopDistance = 3f;

    private Transform playerTransform;

    private void Start()
    {
        if (PlayerLocatorSingleton.Instance != null)
        {
            playerTransform = PlayerLocatorSingleton.Instance.transform;
        }
    }

    private void Update()
    {
        if (NavMeshAgent.enabled == false||playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stopDistance)
        {
            NavMeshAgent.isStopped = false;
            NavMeshAgent.destination = playerTransform.position;
        }
        else
        {
            NavMeshAgent.isStopped = true; // stop movement when in range
        }
    }
}

