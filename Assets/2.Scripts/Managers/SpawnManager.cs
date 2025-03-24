using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject monsterPrefab; // 생성할 몬스터 프리팹
    public Transform spawnPoint; // 몬스터 생성 위치
    [SerializeField] private int spawnCount = 5;


    private void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        float spawnInterval = Random.Range(1f, 5f);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            monster.name = $"Monster_{i}";
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}