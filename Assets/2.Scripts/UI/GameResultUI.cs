using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private GameObject gameResultUI;
    [SerializeField] private GameObject newRecord;
    [SerializeField] private TMP_Text currentRecordText;
    [SerializeField] private TMP_Text lastBestRecordText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� �����
    public void GameStart(GameObject ui)
    {
        if (StartCheck())
        {
            GameManager.Instance.MoneyChange(Constants.MoneyType.KEY, -1);
            SceneManager.LoadScene("GameScene");
            Time.timeScale = 1f;
        }
        else
            GameManager.Instance.PopUpController.UIOn(ui);
    }

    // ��ȭ Ȯ��
    private bool StartCheck()
    {
        return GameManager.Instance.Key > 0;
    }

    // ���� ȭ������ �̵�
    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
}
