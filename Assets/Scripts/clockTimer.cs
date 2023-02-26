using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockTimer : MonoBehaviour
{
    public class timer
    {
        public bool timerOn = false;
        public float timerDuration;
        public float timerTime = 0;

        public void ResetTimer()
        {
            timerTime = 0;
        }
        public void SetTimerTime(float timeAmount)
        {
            timerTime = timeAmount;
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
            while (timerOn && timerTime > timerDuration)
            {
                timerTime -= Time.deltaTime;
            }

            timerOn = false;
        }
    }
}
