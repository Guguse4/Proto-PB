using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera topDownCam;
    public CinemachineCamera sideCam;

    void Start()
    {
        SwitchToTopDown();
    }


    public void SwitchToTopDown()
    {
        topDownCam.Priority = 10;
        sideCam.Priority = 0;
    }

    public void SwitchToSide2D()
    {
        topDownCam.Priority = 0;
        sideCam.Priority = 10;
    }
}
