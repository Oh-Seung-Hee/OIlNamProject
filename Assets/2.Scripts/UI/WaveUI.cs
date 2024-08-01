using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private int maxWave = 50;
    [SerializeField] public int currentWave = 1;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TimerUI timerUI;
    public bool isBossWave = false;

    private EnemySpawn enemySpawn;
    private DataTable_ChapterLoader chapterDatabase;

    //private void Start()
    //{
    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.Instance.WaveUI = this;
    //    }
        
    //    timerUI = timerUI.GetComponent<TimerUI>();
    //    enemySpawn = GetComponent<EnemySpawn>();

    //    StartWave();
    //}

    public void Init()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WaveUI = this;
        }

        timerUI = timerUI.GetComponent<TimerUI>();
        enemySpawn = GetComponent<EnemySpawn>();
        chapterDatabase = enemySpawn.chapterDatabase;

        StartWave();
    }

    // Wave ����
    private void StartWave()
    {
        timerUI.Init();
        UpdateWaveUI();
    }

    // ���� Wave�� ����
    public void NextWave()
    {
        enemySpawn.gameSceneManager.happyEnergy.HappyEnergyCheck();//�ӵ�, ���ݷ� nowChange true���� �ٲٴ°��߿� ���� �Ʒ��ο;���

        currentWave++;

        // �Ѿ ���� Wave�� ���� ��
        if(currentWave <= maxWave)
        {
            int tmpWave = currentWave % 10;
            int setTime = chapterDatabase.GetByKey(currentWave).Time;

            UpdateWaveUI();

            //timerUI.SetTimer(setTime);
            //enemySpawn.RestartSpawnEnemy(enemyCount, currentWave);

            // ������ �Ϲ� Wave�� ��
            if (tmpWave != 0)
            {
                int enemyCount = chapterDatabase.GetByKey(currentWave).EnemyCount;

                timerUI.SetTimer(setTime);
                enemySpawn.RestartSpawnEnemy(enemyCount, currentWave);
            }
            // ������ ���� Wave�� ��
            else if(tmpWave == 0 && currentWave != 50)
            {
                int bossCount = chapterDatabase.GetByKey(currentWave).BossCount;

                timerUI.SetTimer(setTime);
                // **** TODO : �������� ��ȯ �����ϱ� ****
                enemySpawn.RestartSpawnEnemy(bossCount, currentWave, true);
            }
            // ������ 50 Wave�� ��
            else
            {
                int enemyCount = chapterDatabase.GetByKey(currentWave).EnemyCount;
                int bossCount = chapterDatabase.GetByKey(currentWave).BossCount;

                timerUI.SetTimer(setTime);
                enemySpawn.RestartSpawnEnemy(enemyCount, currentWave);
                enemySpawn.RestartSpawnEnemy(bossCount, currentWave, true);
            }
        }
    }

    // wave UI ������Ʈ
    private void UpdateWaveUI()
    {
        waveText.text = "WAVE " + currentWave.ToString();
    }
}
