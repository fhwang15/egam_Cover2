using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum FishState
{
    Patroling,
    Chasing,
}


public class FishCondition : MonoBehaviour
{

    public Fish fish; //What type of Fish

    public bool isCaught; //See if it is caught or not.

    public float bobbing; //몇번 톡톡 하는지 Random.Range를 통해 만들어질겅미~ Random.Range(0,6)
    private float fishBite; //Check how many times it has bitten.

    public float weight; //애니메이션의 길이. 무거울수록 길어긴다.
    public float speed; //시간 있으면 설정할거. 물고기가 얼마나 빨리움직쓰하는지

    public FishAI fishAi;

    public FishState currentstate = FishState.Patroling;


    public float detectionRadius = 1f;
    public float detectionRange = 2f;
    public Transform rayorigin;
    public LayerMask targetLayer;


    void Start()
    {
        fishBite = 0;

        bobbing = Random.Range(1, fish.bobbing);
        weight = fish.weight;

        speed = fish.speed;

        fishAi = GetComponent<FishAI>();

        isCaught = false;
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

        }

    }



    public void ChangeState(FishState newState)
    {
        if (currentstate != newState)
        {
            currentstate = newState; // 새로운 상태로 변경

            // 상태에 맞는 행동을 `FishAI`에 위임
            switch (newState)
            {
                case FishState.Patroling:
                    fishAi.Patrol();
                    break;
                case FishState.Chasing:
                    fishAi.Chasing();
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lure")
        {
            if(fishBite < bobbing)
            {
                Vector3 attackDirection = transform.position - collision.gameObject.transform.position;
                

                fishBite = fishBite + 1;
                fishAi.ApplyKnockback(attackDirection);
            
            } 
            else if (fishBite >= bobbing)
            {
                isCaught = true;
                Destroy(gameObject);
            }
        }
    }

}
