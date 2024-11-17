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

    public float bobbing; //��� ���� �ϴ��� Random.Range�� ���� ��������Ϲ�~ Random.Range(0,6)
    private float fishBite; //Check how many times it has bitten.

    public float weight; //�ִϸ��̼��� ����. ���ſ���� �����.
    public float speed; //�ð� ������ �����Ұ�. ����Ⱑ �󸶳� �����������ϴ���

    public FishAI fishAi;

    public FishState currentstate = FishState.Patroling;


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
                if(fishAi != null)
                {
                    fishAi.ResumePatrol();
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
            currentstate = newState; // ���ο� ���·� ����

            // ���¿� �´� �ൿ�� `FishAI`�� ����
            switch (newState)
            {
                case FishState.Patroling:
                    fishAi.ResumePatrol();
                    break;
                case FishState.Chasing:
                    fishAi.Chasing();
                    break;
            }
        }
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
