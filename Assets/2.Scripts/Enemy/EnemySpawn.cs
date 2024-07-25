using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    //[SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    private List<Enemy> enemyList;
    private int wavePerMax = 30;
    private int currentCount = 0;

    EnemyMove enemyMove;

    private void Awake()
    {
        // ������ ������ �־���� ����Ʈ �Ҵ�
        enemyList = new List<Enemy>();

        //enemyMove = enemyPrefab.GetComponent<EnemyMove>();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // SpawnPoint���� ���� ����
    private IEnumerator SpawnEnemy()
    {
        // �̹� Wave�� ���� �����Ǿ�� �� ������ �ִٸ�
        while (currentCount <= wavePerMax)
        {
            CreateEnemy();

            yield return new WaitForSeconds(2.0f);
        }

        currentCount = 0;
    }

    // ������ �����Ѵ�.
    private void CreateEnemy()
    {
        // ********** TODO : ������Ʈ Ǯ�� ����� ���� �����Ǿ�� ��! ������ �켱 ������Ʈ�� �����ϴ� �����. **********
        // ������ ���� ������ �޾ƿ��� �Ϳ� ���ؼ��� ���� �� ��
        //Instantiate(enemyPrefab, wayPoints[0].position, Quaternion.identity);
        //currentCount++;
        //enemyList.Add(enemyPrefab);

        //enemyMove = enemyPrefab.GetComponent<EnemyMove>();
        //enemyMove.Init(wayPoints);

        GameObject clone = Instantiate(enemyPrefab, wayPoints[0].position, Quaternion.identity);
        Enemy enemy = clone.GetComponent<Enemy>();
        enemy.Init(1);
        enemyMove = enemy.enemyMove;

        enemyMove.Init(wayPoints);
        enemyList.Add(enemy);
    }
}
