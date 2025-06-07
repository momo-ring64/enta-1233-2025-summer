using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToTransform : MonoBehaviour
{

    [SerializeField] private Transform MoveToPoint;
    [SerializeField] private NavMeshAgent NavMeshAgent;

    // Start is called before the first frame update
    void Update()
    {
        NavMeshAgent.destination = PlayerLocatorSingleton.Instance.transform.position;
    }

    // Update is called once per frame
    
}
