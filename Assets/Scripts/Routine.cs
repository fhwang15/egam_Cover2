using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Detect the Fishing spot
    /// Raycast or OnTriggerEnter 
    /// If the player is within the ����, �׷��� fishing rod�� ���� ��ȸ ��ü�� �־�����. canThrow bool�� true����

    //Throw the fishing rod to the fishing spot (Distance determined by how long you press) (casting)
    ///canThrow�� true��, �����̽��� �������μ� ����������. float throwDistance�� += 0.01 �ؼ� distance�� �����. �ִ� 5������ �ϴ� ����
    ///���� ������ ���� layer water�̸� fishing state�� ����ǰ�, �ƴҽ� �ƹ��͵� ���ؼ� retrieve�ؾ��Ѵ�.
    ///rodIsThrown�� true�� �����̽��� �������μ� false�� ���������. (�̰� ������... enum���� �ɵ�.)

    //Wait for the fish to get caught (Bobbing)
    ///rodIsThrown state�� fishing state�� ���������� ���������� ��ٷ�����
    ///���� n���̻� �� ������ ����. (stretch) ������ ���� ���̽��� �ֱ�

    //The Fish will be caught (Single type of fish for now) (hooked)
    /// Scriptable object for the fish
    /// 


    //Player will have to get into bobbing phase ==> press space to ziral  (Reel in)
    ///float reelin increases whenever space is pressed.

    //Catch the fish
    /// if reelin >= fish's weight variable, then fish has been caught.
    /// 
    

    //�İ��� ã����
    ///�ڤ����Դ¹�
    ///��... �������� �ڷ�ƾ ����
    /// Scriptable object
    ///

}
