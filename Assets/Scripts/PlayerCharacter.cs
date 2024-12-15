using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public enum characterState
{
    Default,
    Throwing,
    Catching
}

public class PlayerCharacter : MonoBehaviour
{
    public static bool CatchFish = false;
    public static bool restartFishing = false;


    public GameObject lure;
    public GameObject currentLure;

    public Rigidbody rb;
    public float speed;

    characterState characterState;

    Vector3 movement;

    bool isThrown; //I want to throw this one time
    bool isSuccessful;


    public float CaughtMaxTime; //Maximum time that the fish will stay when you are reelingup
    public float CaughtPlayerTime;
    public float playerPower = 1f;

    public float currentPlayerPower = 0; //Determines player power.


    public GameObject ResultText;
    public GameObject FishTimer;
    public GameObject currentPower;

    public GameObject ResetButton;
    public GameObject Press;


    public TextMeshProUGUI WinOrLose;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI PlayerText;

    public TextMeshProUGUI Instruction;

    public FishInfo SquareFish;


    public Coroutine fishingRoutine;



    // Start is called before the first frame update
    void Start()
    {


        CaughtMaxTime = 12f;

        isThrown = false;
        characterState = characterState.Default;
        ResetButton.SetActive(false);

        rb = GetComponent<Rigidbody>();

        CatchFish = false;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(characterState.ToString());

        if (FishCondition.isCaught)
        {
            characterState = characterState.Catching;
        }

        switch (characterState)
        {
            case characterState.Default:

                if ((int)characterState == 0 || (int)characterState == 1)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        characterState++;

                        if ((int)characterState > 1)
                        {
                            characterState = 0; //returns to default state.
                        }
                    }
                }

                DefaultState();
                break;
            case characterState.Throwing:

                if ((int)characterState == 0 || (int)characterState == 1)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        characterState++;

                        if ((int)characterState > 1)
                        {
                            characterState = 0; //returns to default state.
                        }
                    }
                }

                ThrowingState();
                break;
            case characterState.Catching:
                CatchingState();
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



    void ThrowingState()
    {
        if(fishingRoutine == null)
        {
            rb.velocity = Vector3.zero; //Does not allow the player to move.
            if (!isThrown)
            {
                ThrowingLure();
                isThrown = true;
            }

           //fishingRoutine = StartCoroutine(ExecuteGamePlay()); //Once character throws the rod, enter the coroutine.
        }
     
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

    void CatchingState()
    {
        if (!CatchFish)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CatchFish = true;
            }
        }
        return;
    }

}
