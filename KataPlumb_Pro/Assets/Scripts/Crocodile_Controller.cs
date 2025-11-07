using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crocodile_Controller : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject goalDestination;
    public float speedClose = 1f;
    public float speedMidle = 5f;
    public float speedFar = 10f;

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
        if (Vector3.Distance(transform.position,goalDestination.transform.position) < 9)
        {
            ChangeSpeed(0);
        }
        else if (Vector3.Distance(transform.position, goalDestination.transform.position) < 17)
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
                actualSpeed = speedClose;
                break;
            case 1:
                actualSpeed = speedMidle;
                break;
            case 2:
                actualSpeed = speedFar;
                break;
        }
        navMeshAgent.speed = actualSpeed;
        Debug.Log(actualSpeed);
    }
}
