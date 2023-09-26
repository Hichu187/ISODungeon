using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemies : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
         
    }
    void Update()
    {
        agent.SetDestination(target.transform.position);


    }

    private void OnMouseDown()
    {
        _PlayerState.instance.target = this.gameObject;
    }
}
