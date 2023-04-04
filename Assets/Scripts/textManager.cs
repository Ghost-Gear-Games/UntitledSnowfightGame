using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textManager : MonoBehaviour
{
    //Gameobjects
    public GameObject currencyCountObject;
    public GameObject waveCountObject;
    public attackManager atkMgr;
    public healthSystem playerHPSystem;

    //Text Components
    public TextMeshProUGUI currencyCountText;
    public TextMeshProUGUI waveCountText;

    public TextMeshProUGUI snowballUpCost;
    public TextMeshProUGUI yellowSnowballUpCost;
    public TextMeshProUGUI slushUpCost;
    public TextMeshProUGUI healCost;

    //User Variables
    public playerControl playerCntrl;
    public waveControl waveCntrl;


    void Start()
    {
        currencyCountText = currencyCountObject.GetComponent<TextMeshProUGUI>();
        waveCountText = waveCountText.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        currencyCountText.text = "$" + playerCntrl.lunchMoneyTotal.ToString();

        waveCountText.text = "Wave #" + waveCntrl.currentWave.ToString();

        snowballUpCost.text ="Level:" + (playerCntrl.snowball.upgradeCount + 1) + " $" + playerCntrl.snowball.upgradeCost;

        yellowSnowballUpCost.text = "Level:" + (playerCntrl.yellowSnowball.upgradeCount + 1) + " $" + playerCntrl.yellowSnowball.upgradeCost;

        slushUpCost.text = "Level:" + (playerCntrl.slush.upgradeCount + 1) + " $" + playerCntrl.slush.upgradeCost;

        healCost.text = "$" + (10 * Mathf.Pow(2, playerHPSystem.HealCount)).ToString();
    }


}
