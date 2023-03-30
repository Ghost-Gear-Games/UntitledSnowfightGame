using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditScroller : MonoBehaviour
{
    public RectTransform creditTf;
    [SerializeField]
    int scrollSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(scrollCredits());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator scrollCredits()
    {
        while ((creditTf.localPosition.y < -100))
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                scrollSpeed = 3;
            }
            else
            {
                scrollSpeed = 1;
            }
            creditTf.SetPositionAndRotation(creditTf.position + new Vector3(0, scrollSpeed), creditTf.rotation);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
