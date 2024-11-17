using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public Fish fish;


    bool isCaught; //See if it is caught or not.

    

    public float bobbing; //몇번 톡톡 하는지 Random.Range를 통해 만들어질겅미~
    public float weight; //얼마나 많이 처 resist할건지

    public float speed; //시간 있으면 설정할거. 물고기가 얼마나 빨리움직쓰하는지

    // Start is called before the first frame update
    void Start()
    {
        isCaught = false;
        Debug.Log(fish.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCaught = true;
        }
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {

        yield return new WaitForSeconds(3);

        if (isCaught)
        {
            Debug.Log("It works!");
        } else if(!isCaught) 
        {
            Debug.Log("Give Up");
        }

    }


}
