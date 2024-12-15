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
    bool isWaiting = false; //waiting for the next random movement to be done.
    bool isReached = true; //waiting to be reached?

    private NavMeshAgent agent;
    private Vector3 targetPosition;

    public FishCondition bobbing;

    bool isKnockedBack = false;
    public float knockbackDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //AI세팅해서 대충 가져오기
        NextDestination(); //먼저 갈 곳을 정해
    }

    void Update()
    {

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


    void NextDestination()
    {
        if (isReached) //Don't look for random destination until it reaches the destination.
        {
            patrollingRadius = Random.Range(1, 7);
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

   

    public void ApplyKnockback(Vector3 sourcePosition)
    {
        if (isKnockedBack) return;

        // 넉백 방향 계산
        Vector3 knockbackDirection = (transform.position - sourcePosition).normalized;

        // 넉백 실행
        StartCoroutine(KnockbackRoutine(knockbackDirection));
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


    private IEnumerator KnockbackRoutine(Vector3 direction)
    {
        isKnockedBack = true;

        // NavMesh 에이전트 비활성화 없이 넉백 적용
        agent.isStopped = true;
        float timer = 0f;
        float knockbackStrength = 3; 
        
        Vector3 initialPosition = transform.position;

        // 넉백 효과가 끝날 때까지 루프
        while (timer < knockbackDuration)
        {
            // 넉백 방향에 따라 물고기의 속도 설정
            Vector3 knockbackVelocity = direction * knockbackStrength;
            agent.velocity = knockbackVelocity;

            // 넉백 종료 조건: 지정한 시간 동안 이동
            timer += Time.deltaTime;

            yield return null;
        }

        bobbing.fishBite++;
   
        // NavMesh 에이전트 활성화
        agent.isStopped = false; // 이동 재개

        // 경로 재탐색 (다시 미끼를 쫓아가도록 설정)
        agent.SetDestination(FollowingLure.currentLure.transform.position);

        isKnockedBack = false;

        yield return null;
    }


}
