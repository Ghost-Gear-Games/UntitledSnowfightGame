using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class creditScroller : MonoBehaviour
{
    public RectTransform creditTf;
    [SerializeField]
    double scrollSpeed = 0.5;
    Scrollbar scrollBar;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {


    }
    void MoveCreditsUp()
    {
        creditTf.SetPositionAndRotation(creditTf.position + new Vector3(0, 1), creditTf.rotation);
    }
    void MoveCreditsDown()
    {
        creditTf.SetPositionAndRotation(creditTf.position + new Vector3(0, -1), creditTf.rotation);
    }
}
