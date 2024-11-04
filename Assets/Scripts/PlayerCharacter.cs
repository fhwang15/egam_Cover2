using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public enum characterState
{
    Default,
    Fishing,

}

public class PlayerCharacter : MonoBehaviour
{
    public GameObject lure;
    public GameObject currentLure;
    
    public Rigidbody rb;
    public float speed;

    characterState characterState;

    Vector3 movement;

    bool isThrown; //I want to throw this one time
    bool isSuccessful;

    bool beforeCaught;
    bool canIncreasePower = true;


    public float CaughtMaxTime; //Maximum time that the fish will stay when you are reelingup
    public float CaughtPlayerTime;
    public float playerPower = 1f;

    public float currentPlayerPower = 0;

    //float fishPower = 10f;

    public GameObject ResultText;
    public GameObject FishTimer;

    public TextMeshProUGUI WinOrLose;
    public TextMeshProUGUI TimerText;

    public FishInfo SquareFish;



    // Start is called before the first frame update
    void Start()
    {
        CaughtMaxTime = 10f;

        beforeCaught = true;

        isThrown = false;
        characterState = characterState.Default;

        WinOrLose = ResultText.GetComponent<TextMeshProUGUI>();
        TimerText = FishTimer.GetComponent<TextMeshProUGUI>();

        ResultText.SetActive(false);
        FishTimer.SetActive(false);

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

        if (SquareFish.isCaught && beforeCaught)
        {
            yield return StartCoroutine(ExecuteFishCaught());
        }


        else if (isSuccessful && !beforeCaught)
        {
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(ExecuteWin());
        }
        else if (!isSuccessful && !beforeCaught)
        {
            yield return StartCoroutine(ExecuteLose());
        }

        //isCoroutineRunning = false;
    }

    IEnumerator ExecuteFishCaught()
    {
        FishTimer.SetActive(true);
        CaughtPlayerTime = CaughtMaxTime;
        beforeCaught = true;

        if (Input.GetKey(KeyCode.Z) && canIncreasePower)
        {
            currentPlayerPower = currentPlayerPower + playerPower;
            Debug.Log(currentPlayerPower);
            canIncreasePower = false;

            StartCoroutine(ResetPowerIncrement());
        }

        while (CaughtPlayerTime > 0)
        {

            Timer();

            CaughtPlayerTime -= Time.deltaTime;

            yield return null;
        }


        isSuccessful = currentPlayerPower > SquareFish.fishPower;
        beforeCaught = false;
        FishTimer.SetActive(false);
    }

    IEnumerator ResetPowerIncrement()
    {
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        canIncreasePower = true; // Allow increment again
    }

    void Timer()
    {
        int min = Mathf.FloorToInt(CaughtPlayerTime / 60);
        int sec = Mathf.FloorToInt(CaughtPlayerTime % 60);


        TimerText.text = string.Format("{0:00}:{1:00}", min, sec);

        Debug.Log($"CaughtPlayerTime: {CaughtPlayerTime}");

        

    }


    IEnumerator ExecuteWin()
    {
        ResultText.SetActive(true);
        WinOrLose.text = "Hurrah! You caught the Square Fish!";

        Debug.Log(currentPlayerPower);

        yield return null;
    }

    IEnumerator ExecuteLose()
    {
        ResultText.SetActive(true);
        WinOrLose.text = "The SquareFish Ran Away...";

        Debug.Log(currentPlayerPower);

        yield return null;
    }

    void CatchFish()
    {
        CaughtMaxTime -= Time.deltaTime;

        


    }



    //Can only move around when they are in Default State.




    //���� �� pond�� hit�ϸ� ��ٷ� �ڷ�ƾ���� �����Ѵ�.
    //���� �ó�����
    //��(������ patrol �׸��� ���󰡱� �װ� ���� state machine enum)
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
