using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    public bool timerOn = false;
    public float timerDuration;
    public float timerTime = 0;
    public void ResetTimer()
    {
        timerTime = 0;
    }
    public void TurnOnTimer()
    {
        timerOn = true;
    }
    public void StartTimerCountUp()
    {
        while (timerOn && timerTime < timerDuration)
        {
            timerTime += Time.deltaTime;
        }
        timerOn = false;
    }
    public void StartTimerCountDown()
    {
        while(timerOn && timerTime > timerDuration)
        {
            timerTime -= Time.deltaTime;
        }
        timerOn = false;
    }
    public void StopTimer()
    {
        timerOn = false;
    }
}
