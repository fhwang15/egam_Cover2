using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FishingCamera : MonoBehaviour
{

    public CinemachineVirtualCamera zoomCamera;
    public float zoomDuration = 2.0f;
    private CinemachineVirtualCamera defaultCamera;


    // Start is called before the first frame update
    void Start()
    {
        defaultCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        zoomCamera.Priority = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnFishCaught()
    {
        zoomCamera.Priority = 10;
    }

    public void ResetCamera()
    {
        
        zoomCamera.Priority = 0;
    }


}
