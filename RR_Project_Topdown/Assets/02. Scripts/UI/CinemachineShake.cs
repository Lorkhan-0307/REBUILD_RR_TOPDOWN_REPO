using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoSingleton<CinemachineShake>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimeTotal;
    private float startingIntensity;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimeTotal));
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimeTotal = time;
        shakeTimer = time;
    }

}
