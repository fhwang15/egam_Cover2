using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInfo : MonoBehaviour
{

    public Fish fish;

    public bool isCaught; //See if it is caught or not.

    public float bobbing; //몇번 톡톡 하는지 Random.Range를 통해 만들어질겅미~ Random.Range(0,6)
    public float weight; //애니메이션의 길이. 무거울수록 길어긴다.

    public float speed; //시간 있으면 설정할거. 물고기가 얼마나 빨리움직쓰하는지

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCaught)
        {
            this.gameObject.SetActive(false);
        } 
        
        else if (!isCaught)
        {
            this.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Lure")
        {
            isCaught = true;
        }
    }
}
