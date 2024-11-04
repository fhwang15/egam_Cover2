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
    /// If the player is within the 범위, 그러면 fishing rod를 던질 기회 자체가 주어진다. canThrow bool이 true가댐

    //Throw the fishing rod to the fishing spot (Distance determined by how long you press) (casting)
    ///canThrow가 true면, 스페이스를 누름으로서 던질수있음. float throwDistance가 += 0.01 해서 distance가 변경됨. 최대 5정도로 일단 세팅
    ///만약 떨어진 곳이 layer water이면 fishing state로 변경되고, 아닐시 아무것도 안해서 retrieve해야한다.
    ///rodIsThrown이 true면 스페이스를 누름으로서 false로 만들수있음. (이건 뭐랄까... enum쓰면 될듯.)

    //Wait for the fish to get caught (Bobbing)
    ///rodIsThrown state에 fishing state면 물고기새끼가 잡힐때까지 기다려야함
    ///만약 n초이상 안 잡히면 실패. (stretch) 당장은 성공 케이스만 넣기

    //The Fish will be caught (Single type of fish for now) (hooked)
    /// Scriptable object for the fish
    /// 


    //Player will have to get into bobbing phase ==> press space to ziral  (Reel in)
    ///float reelin increases whenever space is pressed.

    //Catch the fish
    /// if reelin >= fish's weight variable, then fish has been caught.
    /// 
    

    //씻고나서 찾을거
    ///자ㄹ살함는법
    ///음... 교수님의 코루틴 강의
    /// Scriptable object
    ///

}
