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
    private EnemySpawn enemySpawn;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WaveUI = this;
        }
        
        timerUI = timerUI.GetComponent<TimerUI>();
        enemySpawn = GetComponent<EnemySpawn>();

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
        if(currentWave < maxWave)
        {
            int tmpWave = currentWave % 10;

            UpdateWaveUI();

            // ������ �Ϲ� Wave�� ��
            if (tmpWave != 0)
            {
                timerUI.SetTimer(30);
                enemySpawn.RestartSpawnEnemy();
            }
            // ������ ���� Wave�� ��
            else
            {
                timerUI.SetTimer(60);
                // **** TODO : �������� ��ȯ �����ϱ� ****
            }
        }
    }

    // wave UI ������Ʈ
    private void UpdateWaveUI()
    {
        waveText.text = "WAVE " + currentWave.ToString();
    }
}
