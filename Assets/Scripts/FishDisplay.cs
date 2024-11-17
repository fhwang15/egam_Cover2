using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public Fish fish;


    bool isCaught; //See if it is caught or not.

    

    public float bobbing; //��� ���� �ϴ��� Random.Range�� ���� ��������Ϲ�~
    public float weight; //�󸶳� ���� ó resist�Ұ���

    public float speed; //�ð� ������ �����Ұ�. ����Ⱑ �󸶳� �����������ϴ���

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
