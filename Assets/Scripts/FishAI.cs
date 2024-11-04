using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishAI : MonoBehaviour
{
    public float patrollingRadius;
    public float waitingTime;

    private NavMeshAgent agent;
    private Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {


        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patrol());
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
    

    IEnumerator Patrol()
    {
        while (true)
        {
            NextDestination();
            yield return new WaitForSeconds(waitingTime);
        }

}
