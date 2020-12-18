using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    // Tunables
    [SerializeField] AudioClip beginningClip = null;
    [SerializeField] AudioClip loopingClip = null;

    // Cached References
    AudioSource audioSource = null;

    private void Awake()
    {
        if (FindObjectsOfType<BackgroundMusicPlayer>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayBackgroundMusic());
    }

    private IEnumerator PlayBackgroundMusic()
    {
        audioSource.clip = beginningClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = loopingClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
