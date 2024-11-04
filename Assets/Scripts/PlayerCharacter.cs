using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum characterState
{
    Default,
    Fishing,

}

public class PlayerCharacter : MonoBehaviour
{
    public GameObject lure;
    private GameObject currentLure;
    
    public Rigidbody rb;
    public float speed;

    characterState characterState;

    Vector3 movement;

    bool isThrown;

    private Coroutine trackRoutine;

    // Start is called before the first frame update
    void Start()
    {
        isThrown = false;
        characterState = characterState.Default;
       
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterState++;

            if((int)characterState > 1)
            {
                characterState = 0; //returns to default state.
            }
        }

        switch (characterState)
        {
            case characterState.Default:
                DefaultState();
                break;
            case characterState.Fishing:
                FishingState();
                break;
        }

      
    }


    void DefaultState()
    {
        Run(); //If character did not throw the rod, then it will move around.

        if (currentLure != null)
        {
            Destroy(currentLure);
            currentLure = null;
        }

        if (isThrown)
        {
            isThrown = false;
        }
    }

    void Run()
    {
        //Character patorling the map.
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        movement = new Vector3(xAxis, 0, yAxis) * speed;

        rb.velocity = movement;
    }



    void FishingState()
    {
        rb.velocity = Vector3.zero; //Does not allow the player to move.
        if (!isThrown)
        {
            ThrowingLure();
            isThrown = true;
        }

        StartCoroutine(ExecuteGamePlay()); //Once character throws the rod, enter the coroutine.
    }


    void ThrowingLure()
    {
        if(currentLure == null)
        {
            Vector3 spawnPosition = this.transform.position + this.transform.forward;

            currentLure = Instantiate(lure, spawnPosition, this.transform.rotation);
            Rigidbody rb = currentLure.GetComponent<Rigidbody>();

            Vector3 throwAngle = this.transform.forward;
            float throwPower = 5f;

            throwAngle.y = 0.25f;
            rb.AddForce(throwAngle.normalized * throwPower, ForceMode.Impulse);
        }
    }


   

    IEnumerator ExecuteGamePlay()
    {
        //Lure thrown, and waiting for the thing to be caught.
        yield return StartCoroutine(ExecuteWaitForIntro());


        //If Caught, Enter this Coroutine
        yield return StartCoroutine(ExecuteFishCaught());

        yield return null;

    }

    IEnumerator ExecuteWaitForIntro()
    {
        Debug.DrawRay(transform.position, Vector3.forward*1000f, Color.red);

        yield return null;

    }

    IEnumerator ExecuteFishCaught()
    {
        //Camera will close up to what I got.


        yield return null;
    }

    //Can only move around when they are in Default State.

    //게임 플레이의 순서
    //디폴트 상태에서 돌아다닌다. 
    //돌아다니다가도 스페이스를 누르면 피싱스테이트 돌입.
    //ㄴ 스페이스를 누르면 루어가 날아간다. 루어가 바닥이나 어디든 닿으면 isDrawable == true.
    //ㄴ 다시 스페이스를 누르면 isDrawable == false, 그리고 destroy object한다.
    // 디폴트로 돌아가다. 그리고 피싱 스테이트일때는 못움직임.



    //만약 루어가 pond를 hit하면 곧바로 코루틴으로 돌입한다.
    //낚은 시나리오
    //ㄴ(물고기는 patrol 그리고 따라가기 그게 있음 state machine enum)
    //ㄴ씨바아아아아아아아ㅏ알~
    //안 낚은 시나리오
    //ㄴ한 몇초 있다가 자동으로.

    //각자 다른 물고기 종류가 있지만 이건 나중에 scriptable object로 만들기


    //Coroutine 순서
    //낚은 시나리오 (우선 이것만)
    //루어가 던져졌으니 계속 대기.
    // 만약 트리거에 물고기가 걸리면
    // 1. 연타 코루틴에 돌입
    // 1-2. 일정 이상 연타되지 않으면 그냥 뒤져라
    // . 일정 이상 되면 낚음

    //안낚은 시나리오 (스트레치)


}
