using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum FishState
{
    Patroling,
    Chasing,
    Fighting
}


public class FishCondition : MonoBehaviour
{

    public Fish[] randomfish; //What type of Fish

    int fishChooser;
    private Fish fish;

    public static bool isCaught; //See if it is caught or not.
    public bool hasBitten;

    public float bobbing; //몇번 톡톡 하는지 Random.Range를 통해 만들어질겅미~ Random.Range(0,6)
    public float fishBite; //Check how many times it has bitten.

    public FishAI fishAi;

    public FishState currentstate = FishState.Patroling;


    public float detectionRadius = 1f;
    public float detectionRange = 2f;
    public Transform rayorigin;
    public LayerMask targetLayer;

    public FishingCamera fishingCamera;
    public static bool FishIsChecked;

    public GameObject winText;
    public GameObject resultText;
    public TextMeshProUGUI FishName;
    public TextMeshProUGUI FishDescription;

    public GameObject LoseText;

    public GameObject ResetButton;


    void Start()
    {
        fishChooser = Random.Range(0, randomfish.Length);
        fish = randomfish[fishChooser];

        //current bite
        fishBite = 1;

        //how many times it will bite
        bobbing = Random.Range(1, fish.bobbing);

        
        fishAi = GetComponent<FishAI>();

        isCaught = false;
        hasBitten = false;

        winText.SetActive(false);
        LoseText.SetActive(false);
        ResetButton.SetActive(false);   

        FishName.text = fish.fishType;
        FishDescription.text = fish.description;


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
                    StartCoroutine(FishIsCaught());
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

                ChangeState(FishState.Fighting);

            }
        }
    }

    public bool fishIsChecked()
    {
        if (isCaught)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return FishIsChecked = true;
            }
        }

        return false;

    }


    IEnumerator FishIsCaught()
    {
        yield return new WaitForSeconds(1);

        if (PlayerCharacter.CatchFish)
        {
            fishingCamera.OnFishCaught();
            winText.SetActive(true);
            resultText.SetActive(true);

            ResetButton.SetActive(true);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            fishingCamera.ResetCamera();
        }

        else if (!PlayerCharacter.CatchFish)
        {
            LoseText.SetActive(true);
            ResetButton.SetActive(true);
        }

        Destroy(gameObject);

        isCaught = false;
        yield return null;
    }


}
