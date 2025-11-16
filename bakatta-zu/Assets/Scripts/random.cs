using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // 配列で複数のPrefabを保持
    public int spawnCount = 10;
    public float range = 10f;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // ランダムな位置を決定
            Vector3 randomPosition = new Vector3(
                Random.Range(-range, range),
                0,
                Random.Range(-range, range)
            );

            // ランダムにPrefabを選択
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            GameObject selectedPrefab = prefabsToSpawn[randomIndex];

            // 生成
            Instantiate(selectedPrefab, randomPosition, selectedPrefab.transform.rotation);
        }
    }

    void Update()
    {
        // 今回は不要ですが、何か動的に変えたい場合に使えます
    }
}
