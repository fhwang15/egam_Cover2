using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class FishAI : MonoBehaviour
{
    public float patrollingRadius;
    public float waitingTime;
    public float chaseDistance = 5f;

    public PlayerCharacter FollowingLure;

    bool isChasing;

    private NavMeshAgent agent;
    private Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (FollowingLure != null && FollowingLure.currentLure != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, FollowingLure.currentLure.transform.position);
            if (distanceToPlayer <= chaseDistance && !isChasing)
            {
                Chasing();
            }
            else if (distanceToPlayer > chaseDistance && isChasing)
            {
                ResumePatrol();
            }
        }
        else if (isChasing) // Check if the player has been destroyed
        {
            ResumePatrol();
        }
    }


    void NextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrollingRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrollingRadius, NavMesh.AllAreas);
        targetPosition = hit.position;

        agent.SetDestination(targetPosition);  
    }



    void Chasing()
    {
        isChasing = true;
        agent.SetDestination(FollowingLure.currentLure.transform.position);
    }

    void ResumePatrol()
    {
        isChasing = false;
        StartCoroutine(Patrol());
    }



    IEnumerator Patrol()
    {
        while (!isChasing)
        {
            NextDestination();
            yield return new WaitForSeconds(waitingTime);

            if(FollowingLure.currentLure != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, FollowingLure.currentLure.transform.position);
                if (distanceToPlayer <= chaseDistance)
                {
                    Chasing();
                    yield break;
                }
            }

        }
    }

}
