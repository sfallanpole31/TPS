using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("生成怪物")]
    [SerializeField] GameObject enemy;

    [Header("生成間隔")]
    [SerializeField] float spawnTime = 3f;

    [SerializeField] Transform[] spawnPoint;

    [Header("生成數量")]
    [SerializeField] int spawnAmount = 10;

    bool hasBeenTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTrigger)
            return;
        if (other.gameObject.tag == "Player")
        {
            hasBeenTrigger = true;
            StartCoroutine(Spawn());

        }
    }
    IEnumerator Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoint.Length);
            Instantiate(enemy, spawnPoint[spawnPointIndex].position, spawnPoint[spawnPointIndex].rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }

}
