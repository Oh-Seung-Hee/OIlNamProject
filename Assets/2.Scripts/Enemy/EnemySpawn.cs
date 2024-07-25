using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    //[SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TMP_Text enemyCountText;

    public DataManager dataManager;

    private List<Enemy> enemyList;
    private int maxPerWave = 40;    // wave �� �ִ� ���� ��
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
        while (currentCount <= maxPerWave)
        {
            CreateEnemy();

            yield return new WaitForSeconds(0.5f);
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
        enemy.Init(1, dataManager);
        enemyMove = enemy.enemyMove;

        enemyMove.Init(wayPoints);
        enemyList.Add(enemy);

        UpdateEnemyCountUI();
    }

    // ���� �� UI ������Ʈ
    private void UpdateEnemyCountUI()
    {
        enemyCountText.text = enemyList.Count.ToString() + " / 100";
    }
}
