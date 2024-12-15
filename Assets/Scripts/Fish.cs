using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Fish", menuName = "Fish")]


public class Fish : ScriptableObject
{

    public string fishType;
    public string description;

    public float bobbing; //��� ���� �ϴ��� Random.Range�� ���� ��������Ϲ�~
    public float weight; //�󸶳� ���� ó resist�Ұ���

    public float speed; //�ð� ������ �����Ұ�. ����Ⱑ �󸶳� �����������ϴ���

}
