using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class FishAI : MonoBehaviour
{
    public float patrollingRadius;
    public float waitingTime;
    public float chaseDistance = 5f;

    public float reachedRandomPoint = 1f; //�������� �������� �Ѿ�� ���ؼ� �󸶳� ����������ϴ���

    public PlayerCharacter FollowingLure;

    bool isChasing;
    bool isWaiting = false; //waiting for the next random movement to be done.
    bool isReached = true; //waiting to be reached?

    private NavMeshAgent agent;
    private Vector3 targetPosition;


    bool isKnockedBack = false;
    float knockbackForce = 0.5f;
    float knockbackDuration = 0.5f;

    float knockbackTimer = 0f;

    Vector3 knockbackDirection;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //AI�����ؼ� ���� ��������
        NextDestination(); //���� �� ���� ����
    }

    void Update()
    {

        // �˹� ó��
        if (isKnockedBack)
        {
            KnockBack();
        }
    }


    void NextDestination()
    {
        if (isReached) //Don't look for random destination until it reaches the destination.
        {
            patrollingRadius = Random.Range(1, 7);
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * patrollingRadius; // ������ ���� �� ���� ��ġ


            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, patrollingRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(randomDirection);
                isReached = false;
            }
        }
    }

    bool HasReachedDestination()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }

    public void Patrol()
    {
       if (!isWaiting && HasReachedDestination())
       {
                StartCoroutine(WaitForNextDestination());
                isReached = true;
        }
        
    }


    public void Chasing()
    {   
        if (FollowingLure != null && FollowingLure.currentLure != null)
        {
            StartCoroutine(ChasingDetails());
        }
    }



    public void ApplyKnockback(Vector3 attackDirection)
    {
        isKnockedBack = true;
        agent.enabled = false;
        knockbackDirection = attackDirection.normalized;
    }

    void KnockBack()
    {
        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;

            if (knockbackTimer < knockbackDuration)
            {
                transform.position += knockbackDirection * knockbackForce * Time.deltaTime;
            }
            else
            {
                // �˹� ����
                isKnockedBack = false;
                knockbackTimer = 0f;
                agent.enabled = true; // �ٽ� NavMeshAgent Ȱ��ȭ

                // �˹� �� �ٽ� ������ �����ϵ���
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position); // NavMesh ���� ��ȿ�� ��ġ�� ����
                }
                else
                {
                    Debug.LogError("Failed to find valid position on the NavMesh after knockback.");
                }

            }
        }
    }




    IEnumerator WaitForNextDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);
        NextDestination();
        isWaiting = false;

    }

    IEnumerator ChasingDetails()
    {
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(FollowingLure.currentLure.transform.position);
    }


}
