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

    public FishCondition bobbing;

    bool isKnockedBack = false;
    public float knockbackDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //AI�����ؼ� ���� ��������
        NextDestination(); //���� �� ���� ����
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

   

    public void ApplyKnockback(Vector3 sourcePosition)
    {
        if (isKnockedBack) return;

        // �˹� ���� ���
        Vector3 knockbackDirection = (transform.position - sourcePosition).normalized;

        // �˹� ����
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

        // NavMesh ������Ʈ ��Ȱ��ȭ ���� �˹� ����
        agent.isStopped = true;
        float timer = 0f;
        float knockbackStrength = 3; 
        
        Vector3 initialPosition = transform.position;

        // �˹� ȿ���� ���� ������ ����
        while (timer < knockbackDuration)
        {
            // �˹� ���⿡ ���� ������� �ӵ� ����
            Vector3 knockbackVelocity = direction * knockbackStrength;
            agent.velocity = knockbackVelocity;

            // �˹� ���� ����: ������ �ð� ���� �̵�
            timer += Time.deltaTime;

            yield return null;
        }

        bobbing.fishBite++;
   
        // NavMesh ������Ʈ Ȱ��ȭ
        agent.isStopped = false; // �̵� �簳

        // ��� ��Ž�� (�ٽ� �̳��� �Ѿư����� ����)
        agent.SetDestination(FollowingLure.currentLure.transform.position);

        isKnockedBack = false;

        yield return null;
    }


}
