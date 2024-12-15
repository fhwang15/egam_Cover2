using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Fish", menuName = "Fish")]


public class Fish : ScriptableObject
{

    public string fishType;
    public string description;

    public float bobbing; //몇번 톡톡 하는지 Random.Range를 통해 만들어질겅미~
    public float weight; //얼마나 많이 처 resist할건지

    public float speed; //시간 있으면 설정할거. 물고기가 얼마나 빨리움직쓰하는지

}
