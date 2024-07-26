using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private int wave = 50;
    [SerializeField] private int currentWave = 1;
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
        // �Ѿ ���� �Ϲ� Wave�� ���� ��
        currentWave++;
        UpdateWaveUI();
        timerUI.SetTimer();
        enemySpawn.RestartSpawnEnemy();
        // ������ ���� Wave�� ��

        // ���� ���� Wave�� ������ ��

    }

    // wave UI ������Ʈ
    private void UpdateWaveUI()
    {
        waveText.text = "WAVE " + currentWave.ToString();
    }
}
