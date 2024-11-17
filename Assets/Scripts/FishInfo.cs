using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInfo : MonoBehaviour
{

    public Fish fish;

    public bool isCaught; //See if it is caught or not.

    public float bobbing; //��� ���� �ϴ��� Random.Range�� ���� ��������Ϲ�~ Random.Range(0,6)
    public float weight; //�ִϸ��̼��� ����. ���ſ���� �����.

    public float speed; //�ð� ������ �����Ұ�. ����Ⱑ �󸶳� �����������ϴ���

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
