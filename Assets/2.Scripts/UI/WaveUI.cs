using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private int wave = 50;
    [SerializeField] private int currentWave = 1;
    [SerializeField] private TMP_Text waveText;

    // Wave ����
    private void StartWave()
    {

    }

    // ���� Wave�� ����
    public void NextWave()
    {
        // �Ѿ ���� Wave�� ���� ��

        // ���� ���� Wave�� ������ ��

    }

    // wave UI ������Ʈ
    private void UpdateWaveUI()
    {
        waveText.text = "WAVE " + currentWave.ToString();
    }
}
