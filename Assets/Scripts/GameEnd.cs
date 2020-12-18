using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    // Tunables
    [SerializeField] GameObject enemy = null;
    [SerializeField] GameObject doorLock = null;
    [SerializeField] int maxEnemyCount = 10;
    [SerializeField] float spawnPeriod = 2.0f;
    [SerializeField] Transform[] spawnLocations = null;

    // State
    int currentEnemyCount = 0;
    bool runOnce = false;

    // Cached References
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
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                doorLock.SetActive(true);
                audioSource.Play();
                StartCoroutine(DeathSpawn());
            }
        }
    }

    private IEnumerator DeathSpawn()
    {
        while (currentEnemyCount < maxEnemyCount)
        {
            int spawnIndex = UnityEngine.Random.Range(0, spawnLocations.Length);
            Instantiate(enemy, spawnLocations[spawnIndex].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnPeriod);
            currentEnemyCount++;
        }
    }
}
