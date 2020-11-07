using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{

    public static ScreenShakeController Instance { get; private set; }
    public CinemachineVirtualCamera VirtualCam;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float shakeTimer;

    void Awake()
    {

        if(VirtualCam != null)
        {
            virtualCameraNoise = VirtualCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

 

    public void StartShake(float intensity, float length)
    {
        virtualCameraNoise.m_AmplitudeGain = intensity;
        virtualCameraNoise.m_FrequencyGain = 5f;
        shakeTimer = length;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = 0f;
            }
        }
    }
}
