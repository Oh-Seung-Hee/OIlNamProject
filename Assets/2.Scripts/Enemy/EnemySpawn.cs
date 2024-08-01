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
    [SerializeField] private GameObject gameoverPopup;  // ���� ���� UI
    [SerializeField] private GameObject clearUIPopup;   // ���� Ŭ���� UI

    public GameManager gameManager;
    public Player player;
    public DataManager dataManager;//�ӽ�//����
    public GameSceneManager gameSceneManager;//����
    private WaveUI waveUI;
    private LethalEnergy lethalEnergy;//����
    public DataTable_ChapterLoader chapterDatabase;
    //private PopUpController popUpController;

    private List<Enemy> enemyList;
    //private int maxPerWave = 40;    // wave �� �ִ� ���� ��
    private int currentCount = 0;
    public int deadEnemyCount = 0;  // ó���� ���� ��

    EnemyMove enemyMove;

    private void Awake()
    {
        // ������ ������ �־���� ����Ʈ �Ҵ�
        enemyList = new List<Enemy>();

        waveUI = GetComponent<WaveUI>();
        //enemyMove = enemyPrefab.GetComponent<EnemyMove>();

        lethalEnergy = gameSceneManager.GetComponent<LethalEnergy>();
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
        
        //popUpController = gameManager.GetComponent<PopUpController>();
        chapterDatabase = dataManager.dataTable_ChapterLoader;

        waveUI.Init();

        int enemyCount = chapterDatabase.GetByKey(waveUI.currentWave).EnemyCount;
        StartCoroutine(SpawnEnemy(enemyCount, 1));
    }

    // SpawnPoint���� ���� ����
    private IEnumerator SpawnEnemy(int maxPerWave, int waveNum)
    {
        //int maxPerWave = chapterDatabase.GetByKey(waveUI.currentWave).EnemyCount;

        // �̹� Wave�� ���� �����Ǿ�� �� ������ �ִٸ�
        while (currentCount < maxPerWave)
        {
            CreateEnemy(waveNum);

            yield return new WaitForSeconds(0.6f);
        }
    }

    // ������ �����Ѵ�.
    private void CreateEnemy(int waveNum)
    {
        // ********** TODO : ������Ʈ Ǯ�� ����� ���� �����Ǿ�� ��! ������ �켱 ������Ʈ�� �����ϴ� �����. **********
        // ������ ���� ������ �޾ƿ��� �Ϳ� ���ؼ��� ���� �� ��
        //Instantiate(enemyPrefab, wayPoints[0].position, Quaternion.identity);
        //currentCount++;
        //enemyList.Add(enemyPrefab);

        //enemyMove = enemyPrefab.GetComponent<EnemyMove>();
        //enemyMove.Init(wayPoints);

        int chapterID = waveNum;
        int enemyID = chapterDatabase.GetByKey(chapterID).SpawnEnemy;
        int bossID = chapterDatabase.GetByKey(chapterID).SpawnBoss;

        GameObject clone = Instantiate(enemyPrefab, wayPoints[0].position, Quaternion.identity);
        Enemy enemy = clone.GetComponent<Enemy>();

        if (enemyID > -1)
        {
            enemy.Init(enemyID, chapterID, gameSceneManager, dataManager);//����
            enemyList.Add(enemy);
            currentCount++;
        }
        if (bossID > -1)
        {
            enemy.Init(bossID, chapterID, gameSceneManager, dataManager);
        }

        enemyMove = enemy.enemyMove;

        enemyMove.Init(enemyID, bossID, wayPoints);

        if (enemyList.Count >= 100)
        {
            GameOver();
        }

        UpdateEnemyCountUI();
    }

    // ���� ���� �ڷ�ƾ �����
    public void RestartSpawnEnemy(int enemyCount, int waveNum)
    {
        currentCount = 0;
        //int enemyCount = chapterDatabase.GetByKey(waveNum).EnemyCount;

        StartCoroutine(SpawnEnemy(enemyCount, waveNum));
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
        lethalEnergy.ChangeEnergy(1);

        // ��ȯ�� ������ �� �׾��� ��
        if (enemyList.Count == 0)
        {
            waveUI.NextWave();
        }
    }

    // ���� �� UI ������Ʈ
    private void UpdateEnemyCountUI()
    {
        enemyCountText.text = enemyList.Count.ToString() + " / 100";
    }

    // ������ 100������ �Ѿ�� ���� ����
    private void GameOver()
    {
        GameManager.Instance.PopUpController.PauseUIOn(gameoverPopup);

    }
    
    // ���� Ŭ����
    private void GameClear()
    {
        GameManager.Instance.PopUpController.PauseUIOn(clearUIPopup);
    }
}
