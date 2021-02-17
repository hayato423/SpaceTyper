using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform myRectTfm;
    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //自身の向きをカメラに向ける               
        myRectTfm.LookAt(Camera.main.transform);
        float angleY = myRectTfm.localEulerAngles.y;
        myRectTfm.Rotate(0, 180, 0);
    }
}
