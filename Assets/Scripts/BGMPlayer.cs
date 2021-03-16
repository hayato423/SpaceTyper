using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource bgm;
    private float volume;
    void Start()
    {
        bgm.loop = true;
        volume = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        bgm.volume = volume;
    }
}
