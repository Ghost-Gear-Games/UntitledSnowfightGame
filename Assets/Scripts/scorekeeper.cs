using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scorekeeper : MonoBehaviour
{
    public playerControl plyrCntrl;
    public healthSystem hpSys;
    public waveControl waveCntrl;

    public TextMeshProUGUI scoreText;
    int ScoreCalculation()
    {
        int score = 0;
        score += plyrCntrl.lunchMoneyTotal;
        score += (plyrCntrl.snowball.upgradeCount * 5);
        score += (plyrCntrl.yellowSnowball.upgradeCount * 10);
        score += (plyrCntrl.slush.upgradeCount * 15);
        score -= (hpSys.HealCount * 5);
        score += (waveCntrl.currentWave * 15);
        return score;
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreText.isActiveAndEnabled == true)
        {
            scoreText.text = "Score:" + ScoreCalculation().ToString();
        }
    }
}
