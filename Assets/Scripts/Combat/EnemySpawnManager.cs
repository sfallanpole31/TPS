using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("�ͦ��Ǫ�")]
    [SerializeField] GameObject enemy;

    [Header("�ͦ����j")]
    [SerializeField] float spawnTime = 3f;

    [SerializeField] Transform[] spawnPoint;

    [Header("�ͦ��ƶq")]
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
