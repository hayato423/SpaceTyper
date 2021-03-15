using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource _bgm;
    private float _volume;
    void Start()
    {
        _bgm.loop = true;
        _volume = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _bgm.volume = _volume;
    }
}
