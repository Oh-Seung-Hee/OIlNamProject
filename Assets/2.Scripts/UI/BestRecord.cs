using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestRecord : MonoBehaviour
{
    [SerializeField] private TMP_Text bestRecordText;
    public int bestRecord;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.BestRecord = this;
        }

        Init();
    }

    public void Init()
    {
        bestRecord = 0;
        UpdateBestRecordUI(bestRecord);
    }

    // �ְ� ��� UI ������Ʈ
    public void UpdateBestRecordUI(int waveNum)
    {
        if (bestRecord < waveNum || waveNum == 0)
        {
            bestRecordText.text = "�ְ� ��� : " + bestRecord.ToString() + " WAVE";
        }
    }
}
