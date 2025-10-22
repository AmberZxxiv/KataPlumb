using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crocodile_Controller : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject goalDestination;
    public float cocodrileClose,cocodrileMidle,cocodrileFar;

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
        if (Vector3.Distance(transform.position,goalDestination.transform.position) < 2)
        {
            ChangeSpeed(0);
        }
        else if (Vector3.Distance(transform.position, goalDestination.transform.position) < 8)
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
                actualSpeed = cocodrileClose;
                break;
            case 1:
                actualSpeed = cocodrileMidle;
                break;
            case 2:
                actualSpeed = cocodrileFar;
                break;
        }
        Debug.Log(actualSpeed);
        navMeshAgent.speed = actualSpeed;
    }
}
