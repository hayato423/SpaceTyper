using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEnemy : MonoBehaviour
{
    private bool isRise;
    private float animationCriteriaTime;
    // Start is called before the first frame update
    void Start()
    {
        isRise = true;
        animationCriteriaTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0, Space.Self);
        if (isRise)
        {
            transform.Translate(0, 0.01f, 0);
        }
        else
        {
            transform.Translate(0, -0.01f, 0);
        }
        if (Time.time - animationCriteriaTime > 2.0f)
        {
            isRise = !isRise;
            animationCriteriaTime = Time.time;
        }
    }
}
