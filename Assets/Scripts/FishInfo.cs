using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInfo : MonoBehaviour
{

    public float fishPower = 10f;

    public bool isCaught;

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
            Debug.Log("Work?");
            isCaught = true;
        }
    }
}
