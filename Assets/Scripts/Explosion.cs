using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private AudioSource explosionSound;
    // Start is called before the first frame update
    void Start()
    {
        explosionSound = GetComponent<AudioSource>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
