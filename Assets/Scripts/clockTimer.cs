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


        public void SetTimerTime(float timeAmount)
        {
            timerTime = timeAmount;
        }
        public void CountTimer(bool CountUp)
        {
            if (CountUp && timerOn && timerTime >= timerDuration)
            {
                {
                    Debug.Log("counted up once");
                    timerTime += Time.deltaTime;

                }


            }
            if (timerTime > timerDuration && CountUp)
            {
                Debug.Log("turned off counting up timer due to its duration bieng met");
                timerOn = false;
            }
            if (!CountUp && timerOn && timerTime <= timerDuration)
            {
                Debug.Log("Counted down once");
                timerTime -= Time.deltaTime;
            }
            if(timerTime < timerDuration && !CountUp)
            {
                Debug.Log("turned off counting down timer due to its duration bieng met");
                timerOn = false;
            }
        }
        
    }
}
