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

    public float reachedRandomPoint = 1f; //다음으로 지점으로 넘어가기 위해서 얼마나 가까워져야하는지

    public PlayerCharacter FollowingLure;

    bool isChasing;
    bool isWaiting = false;
    bool isReached = false;

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

        agent = GetComponent<NavMeshAgent>(); //AI세팅해서 대충 가져오기
        NextDestination(); //먼저 갈 곳을 정해
    }

    void Update()
    {


        if (!isWaiting && HasReachedDestination())
        {
            StartCoroutine(WaitForNextDestination()); //이때부터 계속 코루틴을 돌리기
            isReached = true;
            
        }

        if (isChasing && FollowingLure != null && FollowingLure.currentLure != null)
        {
            agent.SetDestination(FollowingLure.currentLure.transform.position);
        }

       

        // 넉백 처리
        if (isKnockedBack)
        {
            KnockBack();
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
                // 넉백 종료
                isKnockedBack = false;
                knockbackTimer = 0f;
                agent.enabled = true; // 다시 NavMeshAgent 활성화

                // 넉백 후 다시 추적을 시작하도록
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position); // NavMesh 상의 유효한 위치로 워프
                }
                else
                {
                    Debug.LogError("Failed to find valid position on the NavMesh after knockback.");
                }
         
            }
        }
    }



    void NextDestination()
    {
        if (isReached) //Don't look for random destination until it reaches the destination.
        {
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * patrollingRadius; // 지정된 범위 내 랜덤 위치

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






    public void Chasing()
    {
        isChasing = true;
        if (FollowingLure != null && FollowingLure.currentLure != null)
        {
            agent.SetDestination(FollowingLure.currentLure.transform.position);
        }
    }

    public void ResumePatrol()
    {
        isChasing = false;
        StartCoroutine(Patrol());
    }


    IEnumerator WaitForNextDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);
        NextDestination();
        isWaiting = false;

    }


    IEnumerator Patrol()
    {
        while (!isChasing)
        {
            NextDestination(); // 쫓아다니지 않을때 랜덤하게 움직인다.
          
            while (!HasReachedDestination() && !isChasing)
            {
                if (FollowingLure.currentLure != null)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, FollowingLure.currentLure.transform.position);

                    if (distanceToPlayer <= chaseDistance)
                    {
                        Chasing();
                        yield break;
                    }
                }

                yield return null;
            }

            yield return new WaitForSeconds(waitingTime); //새로운 곳으로 움직일때마다 일정 시간동안 기다렸다가 다시찾기 이하생략.

        }
    }


    //StartCoroutine(Patrol()); //빙글빙글 돌아가는 팽이 (는 아니고 걍 돌아댕기기)

}
