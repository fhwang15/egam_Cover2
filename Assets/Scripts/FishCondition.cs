using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public enum FishState
{
    Patroling,
    Chasing,
    Fighting
}


public class FishCondition : MonoBehaviour
{

    public Fish fish; //What type of Fish

    public bool isCaught; //See if it is caught or not.
    public bool hasBitten;

    public float bobbing; //��� ���� �ϴ��� Random.Range�� ���� ��������Ϲ�~ Random.Range(0,6)
    public float fishBite; //Check how many times it has bitten.

    public float weight; //�ִϸ��̼��� ����. ���ſ���� �����.
    public float speed; //�ð� ������ �����Ұ�. ����Ⱑ �󸶳� �����������ϴ���

    public FishAI fishAi;

    public FishState currentstate = FishState.Patroling;


    public float detectionRadius = 1f;
    public float detectionRange = 2f;
    public Transform rayorigin;
    public LayerMask targetLayer;


    void Start()
    {

        //current bite
        fishBite = 1;

        //how many times it will bite
        bobbing = Random.Range(1, fish.bobbing);
        

        //stretch goals
        weight = fish.weight; 
        speed = fish.speed;


        
        fishAi = GetComponent<FishAI>();

        isCaught = false;
        hasBitten = false;



    }

    // Update is called once per frame
    void Update()
    {
        switch (currentstate)
        {
            case FishState.Patroling:
                if (fishAi != null)
                {
                    detectLure();
                    fishAi.Patrol();
                }
                break;


            case FishState.Chasing:
                if (fishAi != null)
                {
                    fishAi.Chasing();
                }
                break;

            case FishState.Fighting:
                if (isCaught)
                {
                    
                }
                break;
        }

    }



    public void ChangeState(FishState newState)
    {
        if (currentstate != newState)
        {
            currentstate = newState; // ���ο� ���·� ����

            // ���¿� �´� �ൿ�� `FishAI`�� ����
            switch (newState)
            {
                case FishState.Patroling:
                    fishAi.Patrol();
                    break;
                case FishState.Chasing:
                    fishAi.Chasing();
                    break;
                case FishState.Fighting:

                    break;
            }
        }
    }


    void detectLure()
    {
        Vector3 direction = transform.forward;

        RaycastHit hit;

        if (Physics.SphereCast(rayorigin.position, detectionRadius, direction, out hit, detectionRange, targetLayer))
        {
            Debug.Log("Detected: " + hit.collider.gameObject.name);

            ChangeState(FishState.Chasing);
        }

        Debug.DrawRay(transform.position, direction * detectionRange, Color.green);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lure") && currentstate == FishState.Chasing)
        {
           
            if (fishBite < bobbing)
            {
                Vector3 attackDirection = transform.position - other.gameObject.transform.position;

                fishAi.ApplyKnockback(attackDirection);
            }
            else if (fishBite > bobbing)
            {
                isCaught = true;
            }
        }
    }

}
