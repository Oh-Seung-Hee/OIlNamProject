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

    [Header("EnergyBar")]
    public Slider energySlider;
    public TMP_Text energyText;

    [Header("PopUP")]
    public GameObject popUp;
    public GameObject clickFalse;

    private string text;

    private float totalTime;
    private int tryCount;
    private float tryTime;
    private float percent;

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
            EnergyCheck();
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
        HappyEnergyInit();
    }

    private void Update()
    {
        if (!onPopup)
            totalTime += Time.deltaTime;

        if (totalTime >= tryTime)
        {
            CallTry();
            totalTime = 0;
        }
    }

    private void EnergyCheck()
    {
        if (1 <= currentEnergyPercent && currentEnergyPercent <= 30)
            SetTryValue(3, 10);
        else if (31 <= currentEnergyPercent && currentEnergyPercent <= 60)
            SetTryValue(5, 25);
        else if (61 <= currentEnergyPercent && currentEnergyPercent <= 80)
            SetTryValue(7, 40);
        else if (81 <= currentEnergyPercent && currentEnergyPercent <= 100)
            SetTryValue(10, 55);
    }

    public void ChangeHappyEnergy(int val)
    {
        Energy = val;
    }

    private void SetTryValue(int tryCount, float percent)
    {
        this.tryCount = tryCount;
        this.percent = percent;
        tryTime = 60 / tryCount;
    }

    private void CallTry()
    {
        Debug.Log("��� ���ư�����");

        int random = Random.Range(0, 100);

        if (random <= percent)
        {
            onPopup = true;
            SetPopUp();
        }
    }

    private void SetPopUp()
    {
        Debug.Log("����");
        popUp.gameObject.SetActive(true);
    }

    public void HappyEnergyCheck()//20�̸��϶� �����̵��ӵ�+5%
    {
        //�̰ɺθ��°����� gameSceneManager.unitController.BadEnergy(5);����
        if (currentEnergyPercent <= 20)
        {
            //����
            onBad = true;
            gameSceneManager.unitController.SpeedChange(5);
            Debug.Log("�طο� ȿ��");
        }
        else if (21 <= currentEnergyPercent && currentEnergyPercent > 100)
        {
            if (onBad)
            {
                onBad = false;
                gameSceneManager.unitController.SpeedChange(-5);
            }
            if (onHappy)
            {
                onHappy = false;
                gameSceneManager.unitController.ATKChange(-30);
            }
            Debug.Log("�طο� ȿ�� ����");
        }
        else if (currentEnergyPercent >= 100)
        {
            onHappy = true;
            gameSceneManager.unitController.ATKChange(30);
            Debug.Log("�̷ο� ȿ��");
        }
    }

    public void ClickMessage(int num)
    {
        switch (num)
        {
            case 1:
                if (gameSceneManager.Gold < 15)
                {
                    StopCoroutine("CoClickFalse");
                    StartCoroutine("CoClickFalse");
                    return;
                }
                gameSceneManager.ChangeGold(-15);
                ChangeHappyEnergy(20);
                Debug.Log("//���ݷ�20�������� //���̺곡���� ��");
                popUp.gameObject.SetActive(false);
                onPopup = false;
                break;
            case 2:
                if (gameSceneManager.Gold < 10)
                {
                    StopCoroutine("CoClickFalse");
                    StartCoroutine("CoClickFalse");
                    return;
                }
                gameSceneManager.ChangeGold(-10);
                ChangeHappyEnergy(20);
                Debug.Log("//��+2 �����̵�-2 �����̵�-2 //����");
                popUp.gameObject.SetActive(false);
                onPopup = false;
                break;
            case 3:
                gameSceneManager.ChangeGold(30);
                ChangeHappyEnergy(-15);
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
