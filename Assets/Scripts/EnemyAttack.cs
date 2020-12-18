using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Tunables
    [SerializeField] float damagePerHit = 1f;
    [SerializeField] AudioClip[] enemyAttackSounds = null;

    // Cached References & States
    Player target = null;
    AudioSource audioSource = null;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void DamageTargetOnAnimation()
    {
        if (target == null) { return; }
        target.DecrementHitPoints(damagePerHit);
        QueueAttackSound();
    }

    private void QueueAttackSound()
    {
        if (audioSource.isPlaying) { return; }
        if (enemyAttackSounds != null)
        {
            AudioClip audioClip = enemyAttackSounds[(int)UnityEngine.Random.Range(0, enemyAttackSounds.Length)];
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void SetCurrentTarget(Player target)
    {
        this.target = target;
    }
}
