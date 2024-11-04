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

    //���� �÷����� ����
    //����Ʈ ���¿��� ���ƴٴѴ�. 
    //���ƴٴϴٰ��� �����̽��� ������ �ǽ̽�����Ʈ ����.
    //�� �����̽��� ������ �� ���ư���. �� �ٴ��̳� ���� ������ isDrawable == true.
    //�� �ٽ� �����̽��� ������ isDrawable == false, �׸��� destroy object�Ѵ�.
    // ����Ʈ�� ���ư���. �׸��� �ǽ� ������Ʈ�϶��� ��������.



    //���� �� pond�� hit�ϸ� ��ٷ� �ڷ�ƾ���� �����Ѵ�.
    //���� �ó�����
    //��(������ patrol �׸��� ���󰡱� �װ� ���� state machine enum)
    //�����پƾƾƾƾƾƾƤ���~
    //�� ���� �ó�����
    //���� ���� �ִٰ� �ڵ�����.

    //���� �ٸ� ����� ������ ������ �̰� ���߿� scriptable object�� �����


    //Coroutine ����
    //���� �ó����� (�켱 �̰͸�)
    //�� ���������� ��� ���.
    // ���� Ʈ���ſ� ����Ⱑ �ɸ���
    // 1. ��Ÿ �ڷ�ƾ�� ����
    // 1-2. ���� �̻� ��Ÿ���� ������ �׳� ������
    // . ���� �̻� �Ǹ� ����

    //�ȳ��� �ó����� (��Ʈ��ġ)


}
