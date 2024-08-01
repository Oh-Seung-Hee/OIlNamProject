using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HappyEnergy : MonoBehaviour
{
    public GameSceneManager gameSceneManager;
    public DataTable_MessageLoader messageDatabase;
    private DataTable_Message currentMessage = new();
    public DataManager tempDataManager;

    [Header("EnergyBar")]
    public Slider energySlider;
    public TMP_Text energyText;

    [Header("PopUP")]
    public GameObject popUp;
    public GameObject clickFalse;
    public TMP_Text message;
    public GameObject slot1;
    private TMP_Text[] text1;
    public GameObject slot2;
    private TMP_Text[] text2;
    public GameObject slot3;
    private TMP_Text[] text3;


    private string text;

    /*
     *  Ȯ���� �ʿ�
        private float totalTime;
        private int tryCount;
        private float tryTime;
        private float percent;*/

    private bool onPopup = false;
    private bool onBad = false;
    private bool onHappy = false;

    public int Energy
    {
        get { return energy; }
        private set
        {
            energy += value;

            if (energy >= totalEnergy)
                energy = totalEnergy;

            energySlider.value = (float)energy / totalEnergy;
            PercentChange();
            TextChange();
            /*EnergyCheck();*/
        }
    }
    private int energy;

    private int totalEnergy;
    private float currentEnergyPercent;


    private void PercentChange()
    {
        currentEnergyPercent = (float)energy / totalEnergy * 100;
    }

    private void TextChange()
    {
        text = "";
        for (int i = 0; i < currentEnergyPercent.ToString().Length; i++)
        {
            text += currentEnergyPercent.ToString()[i];
            text += "\n";
        }
        text += "%";

        energyText.text = text;
    }

    private void HappyEnergyInit()
    {
        totalEnergy = 200;
        Energy = 190;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            messageDatabase = GameManager.Instance.DataManager.dataTable_MessageLoader;
        }
        else
        {
            messageDatabase = tempDataManager.dataTable_MessageLoader;
        }

        HappyEnergyInit();

        text1 = slot1.GetComponentsInChildren<TMP_Text>();
        text2 = slot2.GetComponentsInChildren<TMP_Text>();
        text3 = slot3.GetComponentsInChildren<TMP_Text>();
    }

    private void Update()
    {
        /*        if (!onPopup)
                    totalTime += Time.deltaTime;

                if (totalTime >= tryTime)
                {
                    CallTry();
                    totalTime = 0;
                }*/
    }

    /*    private void EnergyCheck()
        {
            if (1 <= currentEnergyPercent && currentEnergyPercent <= 30)
                SetTryValue(3, 10);
            else if (31 <= currentEnergyPercent && currentEnergyPercent <= 60)
                SetTryValue(5, 25);
            else if (61 <= currentEnergyPercent && currentEnergyPercent <= 80)
                SetTryValue(7, 40);
            else if (81 <= currentEnergyPercent && currentEnergyPercent <= 100)
                SetTryValue(10, 55);
        }*/

    public void ChangeHappyEnergy(int val)
    {
        Energy = val;
    }

    /*    private void SetTryValue(int tryCount, float percent)
        {
            this.tryCount = tryCount;
            this.percent = percent;
            tryTime = 60 / tryCount;
        }*/

    /*    private void CallTry()
        {
            Debug.Log("��� ���ư�����");

            int random = Random.Range(0, 100);

            if (random <= percent)
            {
                onPopup = true;
                SetPopUp();
            }
        }*/

    public void SetPopUp()//���̺�üũ�ؼ��θ��� TODO
    {
        if (onPopup)
            return;

        else
        {
            Debug.Log("����");
            SetMessage();
            popUp.gameObject.SetActive(true);
        }

    }

    private void SetMessage()
    {
        List<int> list = new();

        foreach (var item in messageDatabase.ItemsList)
        {
            list.Add(item.key);
        }

        int temp = Random.Range(0, list.Count);


        currentMessage = messageDatabase.GetByKey(list[temp]);

        message.text = currentMessage.Message;

        text1[0].text = currentMessage.Answer1;
        text1[1].text = currentMessage.Energy1.ToString();
        text1[2].text = currentMessage.Price1.ToString();

        text2[0].text = currentMessage.Answer2;
        text2[1].text = currentMessage.Energy2.ToString();
        text2[2].text = currentMessage.Price2.ToString();

        text3[1].text = currentMessage.Energy3.ToString();
        text3[2].text = currentMessage.Price3.ToString();

    }

    public void HappyEnergyCheck()//20�̸��϶� �����̵��ӵ�+5%
    {
        //�̰ɺθ��°����� gameSceneManager.unitController.BadEnergy(5);����
        if (currentEnergyPercent <= 20)
        {
            //����
            onBad = true;
            gameSceneManager.unitController.SpeedChange(5, false, true);
            Debug.Log("�طο� ȿ��");
        }
        else if (21 <= currentEnergyPercent && currentEnergyPercent > 100)
        {
            if (onBad)
            {
                onBad = false;
                gameSceneManager.unitController.SpeedChange(-5, false, true);
            }
            if (onHappy)
            {
                onHappy = false;
                gameSceneManager.unitController.ATKChange(-30, false, true);
            }
            Debug.Log("�طο� ȿ�� ����");
        }
        else if (currentEnergyPercent >= 100)
        {
            onHappy = true;
            gameSceneManager.unitController.ATKChange(30, false, true);
            Debug.Log("�̷ο� ȿ��");
        }
    }

    public void ClickMessage(int num)
    {
        switch (num)
        {
            case 1:
                if (gameSceneManager.Gold < (currentMessage.Price1 * -1))
                {
                    StopCoroutine("CoClickFalse");
                    StartCoroutine("CoClickFalse");
                    return;
                }
                gameSceneManager.ChangeGold(currentMessage.Price1);
                ChangeHappyEnergy(currentMessage.Energy1);
                gameSceneManager.unitController.ATKChange(20, false, false, true);
                Debug.Log("//���ݷ�20�������� //���̺곡���� ��");
                popUp.gameObject.SetActive(false);
                onPopup = false;
                break;
            case 2:
                if (gameSceneManager.Gold < (currentMessage.Price2 * -1))
                {
                    StopCoroutine("CoClickFalse");
                    StartCoroutine("CoClickFalse");
                    return;
                }
                gameSceneManager.ChangeGold(currentMessage.Price2);
                ChangeHappyEnergy(currentMessage.Energy2);
                gameSceneManager.unitController.ATKChange(2, true);
                Debug.Log("//��+2 �����̵�-2 �����̵�-2 //����");
                popUp.gameObject.SetActive(false);
                onPopup = false;
                break;
            case 3:
                gameSceneManager.ChangeGold(currentMessage.Price3);
                ChangeHappyEnergy(currentMessage.Energy3);
                popUp.gameObject.SetActive(false);
                onPopup = false;
                break;
            default:
                Debug.Log("��ư �޼��� ��ȣ ���� �ȵ�");
                break;
        }
    }

    private IEnumerator CoClickFalse()
    {
        clickFalse.SetActive(true);

        yield return new WaitForSecondsRealtime(2);

        clickFalse.SetActive(false);
    }
}
