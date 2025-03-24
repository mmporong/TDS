using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject monsterPrefab; // ������ ���� ������
    public Transform spawnPoint; // ���� ���� ��ġ
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