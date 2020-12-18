using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCollisionQueue : MonoBehaviour
{
    // State
    bool runOnce = false;

    // Cached Reference
    AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!runOnce)
        {
            runOnce = true;
            audioSource.Play();
        }
    }
}
