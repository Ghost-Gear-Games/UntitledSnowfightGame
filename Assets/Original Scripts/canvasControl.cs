using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class canvasControl : MonoBehaviour
{
    public GameObject textMeshProObject;
    public GameObject legacyTextObject;
    public GameObject Button;

    TextMeshProUGUI textMeshPro;
    Text legacyText;

    //User Variables
    public string stringExample;
    public float floatExample;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = textMeshProObject.GetComponent<TextMeshProUGUI>();
        legacyText = legacyTextObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ExampleTextChanges()
    {
        //TextMeshPro set to 
            //a string
            textMeshPro.text = stringExample;
            //a float or any other non-string data type
            textMeshPro.text = floatExample.ToString();
        //Text(Legacy) set to
            //a string
            legacyText.text = stringExample;
            //a float or any other non-string data type
            textMeshPro.text = floatExample.ToString();
    }
}
