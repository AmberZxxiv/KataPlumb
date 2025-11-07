using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crocodile_Controller : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject goalDestination;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        navMeshAgent.destination = goalDestination.transform.position;
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position,goalDestination.transform.position) < 10)
        {
            ChangeSpeed(0);
        }
        else if (Vector3.Distance(transform.position, goalDestination.transform.position) < 20)
        {
            ChangeSpeed(1);
        }
        else
        {
            ChangeSpeed(2);
        }
    }

    void ChangeSpeed(int distance)
    {
        float actualSpeed = navMeshAgent.speed;

        switch (distance)
        {
            case 0:
                actualSpeed = 1f;
                break;
            case 1:
                actualSpeed = 3f;
                break;
            case 2:
                actualSpeed = 10f;
                break;
        }
        navMeshAgent.speed = actualSpeed;
    }
}
