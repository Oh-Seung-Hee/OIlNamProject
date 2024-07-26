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

    public GameManager gameManager;
    public Player player;
    public DataManager dataManager;//�ӽ�//����
    public GameSceneManager gameSceneManager;//����
    private WaveUI waveUI;

    private List<Enemy> enemyList;
    private int maxPerWave = 40;    // wave �� �ִ� ���� ��
    private int currentCount = 0;
    public int deadEnemyCount = 0;  // ó���� ���� ��

    EnemyMove enemyMove;

    private void Awake()
    {
        // ������ ������ �־���� ����Ʈ �Ҵ�
        enemyList = new List<Enemy>();

        waveUI = GetComponent<WaveUI>();
        //enemyMove = enemyPrefab.GetComponent<EnemyMove>();
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
            player = gameManager.Player;
            GameManager.Instance.EnemySpawn = this;
        }
        else
        {
            player = GameManager.Instance.Player;
        }

        StartCoroutine(SpawnEnemy());
    }

    // SpawnPoint���� ���� ����
    private IEnumerator SpawnEnemy()
    {
        // �̹� Wave�� ���� �����Ǿ�� �� ������ �ִٸ�
        while (currentCount < maxPerWave)
        {
            CreateEnemy();

            yield return new WaitForSeconds(0.6f);
        }
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
        enemy.Init(1, gameSceneManager, dataManager);//����
        enemyMove = enemy.enemyMove;

        enemyMove.Init(wayPoints);
        enemyList.Add(enemy);

        currentCount++;
        UpdateEnemyCountUI();
    }

    // ���� ���� �ڷ�ƾ �����
    public void RestartSpawnEnemy()
    {
        currentCount = 0;
        StartCoroutine(SpawnEnemy());
    }

    // ***�ӽ�*** ������ �׾��� ��
    public void EnemyDie(Enemy enemy, GameObject gameObject)
    {
        // ***** TODO : ���� �׾��� �� ó�� ��� �����ϱ� *****
        //currentCount--;
        deadEnemyCount++;
        player.ExpUp(deadEnemyCount);
        enemyList.Remove(enemy);
        Destroy(gameObject);
        UpdateEnemyCountUI();

        // ��ȯ�� ������ �� �׾��� ��
        if(enemyList.Count == 0)
        {
            waveUI.NextWave();
        }
    }

    // ���� �� UI ������Ʈ
    private void UpdateEnemyCountUI()
    {
        enemyCountText.text = enemyList.Count.ToString() + " / 100";
    }
}
