using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Progress;
using System;
using Random = UnityEngine.Random;
using UnityEditor.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using JetBrains.Annotations;
using Constants;
using TMPro;

[Serializable]
public struct Save_UnitData
{
    public List<UnitSaveData> unitSaveDatas;
    public PieceSaveData pieceData;
}
[Serializable]
public struct PieceSaveData
{
    public int unitPiece;

    public int sPiece;
    public int aPiece;
    public int bPiece;
}

[Serializable]
public class PieceData
{
    public int unitPiece = 0;

    public int sPiece = 0;
    public int aPiece = 0;
    public int bPiece = 0;

    public void UsePiece(PieceType type, int value)
    {
        switch (type)
        {
            case PieceType.Unit:
                unitPiece += value; break;
            case PieceType.STier:
                sPiece += value; break;
            case PieceType.ATier:
                aPiece += value; break;
            case PieceType.BTier:
                bPiece += value; break;
        }
    }

    public void Save(ref PieceSaveData data)
    {
        data.unitPiece = unitPiece;
        data.sPiece = sPiece;
        data.aPiece = aPiece;
        data.bPiece = bPiece;
    }

    public void Load(PieceSaveData data)
    {
        unitPiece = data.unitPiece;
        sPiece = data.sPiece;
        aPiece = data.aPiece;
        bPiece = data.bPiece;
    }
}

[Serializable]
public struct UnitSaveData
{
    public int Key;
    public int Level;
    public int Piece;
    public bool Open;
}

[Serializable]
public class UnitData
{
    //�⺻����
    public int key;
    public int upgradeKey;
    public int stepKey;
    public string name;
    public int tier;
    public float atk;
    public float speed;
    public int range;
    public bool open;

    //��ȭ������
    public int level;
    public int piece;

    //Sprite
    public Sprite profile;
    public GameObject prefabs;

    //���׷��̵� ����
    private DataTable_Upgrade upgradeData;

    public void Init(DataTable_Unit unit, DataTable_Upgrade upgradeData)
    {
        this.upgradeData = upgradeData;

        key = unit.key;
        upgradeKey = unit.UpgradeKey;
        stepKey = unit.StepKey;
        name = unit.Name;
        tier = unit.Tier;
        atk = unit.ATK;
        speed = unit.Speed / 10000;
        range = unit.Range;
        open = unit.Open;

        level = 0;
        piece = 0;

        profile = Resources.Load<Sprite>("Unit/Profile/" + unit.Profile);
        prefabs = Resources.Load<GameObject>("Unit/Prefabs/" + unit.Sprite);
    }

    public void Load(UnitSaveData data, DataTable_Upgrade upgradeData)
    {
        this.upgradeData = upgradeData;

        key = data.Key;
        level = data.Level;
        piece = data.Piece;
        open = data.Open;

        SetStatus();
    }

    private void SetStatus()
    {
        for (int i = 0; i < level; i++)
        {
            atk += upgradeData.ATK[i];
            speed += upgradeData.Speed[i];
            range += upgradeData.Range[i];
        }
    }

    public void Save(ref UnitSaveData data)
    {
        data.Key = key;
        data.Level = level;
        data.Piece = piece;
        data.Open = open;
    }

    //�ʿ��� ���׷��̵� ���� ���� ���� --> ���߿� �ʿ��������
    public int Upgrade()
    {
        //���߿� ĳ���� �������� �ٲ�� �ʿ�
        //piece -= upgradeData.NeedPiece[level];
        atk += upgradeData.ATK[level];
        speed += upgradeData.Speed[level];
        range += upgradeData.Range[level];
        level++;

        return upgradeData.NeedPiece[level - 1];
    }
}

public class UnitManager : Manager
{
    [Header("Temp")]//Data
    public DataManager DataManager;
    private DataTable_UnitLoader unitLoader; // ��������
    private DataTable_UpgradeLoader upgradeLoader; //��������

    [Header("UI")]
    public TMP_Text tabPieceTxt;
    public TMP_Text gachaTabPieceTxt;
    public GameObject falseGacha;
    public GameObject resultUI;
    public TMP_Text resultPieceTxt;
    public List<TMP_Text> resultTierPiece = new();
    public List<TMP_Text> unitTabTierPiece = new();
    public List<GameObject> tierAnim = new();
    public GameObject gachaAnim;

    //[HideInInspector]
    public PieceData pieceData = new();
    private int tempSPiece;
    private int tempAPiece;
    private int tempBPiece;

    //public List<UnitData> unitDataBase = new();
    public Dictionary<int, UnitData> unitDataDic = new();

    //CanSpawnUnit //id�� �������
    private List<int> sTierUnitID = new();
    private List<int> aTierUnitID = new();
    private List<int> bTierUnitID = new();

    public List<UnitInstance> canSpawnUnit = new();

    public override void Init(GameManager gm)
    {
        base.Init(gm);

        unitLoader = GameManager.Instance.DataManager.dataTable_UnitLoader;
        upgradeLoader = GameManager.Instance.DataManager.dataTable_UpgradeLoader;
    }

    private void Start()
    {
        if (GameManager.Instance == null || unitLoader == null)
        {
            unitLoader = DataManager.dataTable_UnitLoader;
            upgradeLoader = DataManager.dataTable_UpgradeLoader;
        }

        if (unitDataDic.Count == 0)
        {
            FirstInit();
        }

        InitTierID();

        //Temp
        //pieceData.unitPiece = 30;
    }

    public void SetUIText(TMP_Text tabTxt, TMP_Text gachaTabTxt, GameObject falseGacha,
        GameObject resultUI, TMP_Text resultPieceTxt, List<TMP_Text> resultTierPiece,
        List<GameObject> tierAnim, GameObject gachaAnim, List<TMP_Text> unitTabTierPiece)
    {
        tabPieceTxt = tabTxt;
        gachaTabPieceTxt = gachaTabTxt;
        this.falseGacha = falseGacha;
        this.resultUI = resultUI;
        this.resultPieceTxt = resultPieceTxt;
        this.resultTierPiece = resultTierPiece;
        this.tierAnim = tierAnim;
        this.gachaAnim = gachaAnim;
        this.unitTabTierPiece = unitTabTierPiece;

        ChangeAllUnitPiece();
    }

    private void FirstInit()
    {
        foreach (var item in unitLoader.ItemsList)
        {
            UnitData newData = new();
            newData.Init(item, upgradeLoader.GetByKey(item.UpgradeKey));

            //unitDataBase.Add(newData);
            unitDataDic.Add(newData.key, newData);
        }
    }

    private void InitTierID()
    {
        foreach (var (key, item) in unitDataDic)
        {
            if (!item.open)
                return;

            switch (item.tier)
            {
                case 1:
                    sTierUnitID.Add(key);
                    break;
                case 2:
                    aTierUnitID.Add(key);
                    break;
                case 3:
                    bTierUnitID.Add(key);
                    break;
            }
        }
    }

    private void AddTierID(int key, int tier)
    {
        switch (tier)
        {
            case 1:
                sTierUnitID.Add(key);
                break;
            case 2:
                aTierUnitID.Add(key);
                break;
            case 3:
                bTierUnitID.Add(key);
                break;
        }
    }

    public UnitData GetByKey(int key)
    {
        return unitDataDic[key];
    }

    // ���� �̱��Ҷ�
    // Ƽ� �����ǻ̱�
    public void GachaUnitPiece(int count)
    {
        if (count * 30 > pieceData.unitPiece)
        {
            falseGacha.SetActive(true);
            return;
        }

        if (resultUI.activeSelf)
            resultUI.SetActive(false);

        int type = 0;

        for (int i = 0; i < count; i++)
        {
            type = Random.Range(1, 4);//0���� ���ָ�����

            switch (type)
            {
                case 1:
                    tempSPiece++; break;
                case 2:
                    tempAPiece++; break;
                case 3:
                    tempBPiece++; break;
            }

            pieceData.UsePiece((PieceType)type, 1);
            pieceData.UsePiece(PieceType.Unit, -30);
        }

        gachaAnim.SetActive(true);

        foreach (var t in tierAnim)
        {
            if (t.activeSelf)
                t.SetActive(false);
        }

        if (tempSPiece >= 1)
            tierAnim[0].SetActive(true);
        else if (tempAPiece >= 1)
            tierAnim[1].SetActive(true);
        else if (tempBPiece >= 1)
            tierAnim[2].SetActive(true);


        ChangeAllUnitPiece();
    }

    //ĳ���� �������� ¥���� �ʿ�
    public void ChangeAllUnitPiece()
    {
        foreach (var (key, unit) in unitDataDic)
        {
            switch (unit.tier)
            {
                case (int)UnitTier.STier:
                    unit.piece = pieceData.sPiece; break;
                case (int)UnitTier.ATier:
                    unit.piece = pieceData.aPiece; break;
                case (int)UnitTier.BTier:
                    unit.piece = pieceData.bPiece; break;
            }
        }

        tabPieceTxt.text = pieceData.unitPiece.ToString() + " / 30";

        if (pieceData.unitPiece >= 30)
            tabPieceTxt.color = Color.cyan;
        else
            tabPieceTxt.color = Color.gray;

        gachaTabPieceTxt.text = pieceData.unitPiece.ToString();
        resultPieceTxt.text = pieceData.unitPiece.ToString();
    }

    //�������� ��� ��Ʈ ����
    public void UsePiece(PieceType type, int val)
    {
        pieceData.UsePiece(type, -val);
        ChangeAllUnitPiece();
        UnitPieceTextUpdate();
    }

    //���� �̱��Ҷ�
    //������ �����̱�
    public void GachaNewUnit(int count)
    {
        int keyCount = 0;

        Dictionary<int, int> newUnit = new();

        for (int i = 0; i < count; i++)
        {
            UnitData newData = RandomNewUnit();

            if (newUnit.ContainsKey(newData.key))
                newUnit[newData.key] += 1;
            else
            {
                newUnit.Add(newData.key, 1);
                keyCount++;//���Ը������ ����
            }
        }

        foreach (var (key, num) in newUnit)
        {
            if (!unitDataDic[key].open)
            {
                unitDataDic[key].open = true;
                unitDataDic[key].piece += (num - 1);
                AddTierID(key, unitDataDic[key].tier);
            }
            else
                unitDataDic[key].piece += num;
        }

        //TODO :: Save�ϱ�
    }

    private UnitData RandomNewUnit()
    {
        float total = 0;

        foreach (var item in upgradeLoader.ItemsList)
        {
            total += (item.Percent / 100f);
        }

        float random = Random.value * total;

        foreach (var item in upgradeLoader.ItemsList)
        {
            random -= item.Percent;

            if (random <= 0)
            {
                return unitDataDic[item.key];
            }
        }

        Debug.Log("�ƹ����ֵ� �̾���������");
        return null;
    }

    //�ΰ��� ���� �̱�
    //Ƽ������� ���ϸ� �� Ƽ���� �������� ���� �̾���
    public UnitData GetRandomUnit(int tier)
    {
        int index;
        UnitData unit = new();

        switch (tier)
        {
            case 1:
                index = Random.Range(0, sTierUnitID.Count);
                unit = unitDataDic[sTierUnitID[index]];
                break;
            case 2:
                index = Random.Range(0, aTierUnitID.Count);
                unit = unitDataDic[aTierUnitID[index]];
                break;
            case 3:
                index = Random.Range(0, bTierUnitID.Count);
                unit = unitDataDic[bTierUnitID[index]];
                break;
            default:
                Debug.Log("���ְ������� ����");
                return null;
        }

        return unit;
    }

    public void ResultSetting(GameObject go = null)
    {
        resultTierPiece[0].text = tempBPiece.ToString();
        resultTierPiece[1].text = tempAPiece.ToString();
        resultTierPiece[2].text = tempSPiece.ToString();

        tempBPiece = 0;
        tempAPiece = 0;
        tempSPiece = 0;

        resultUI.SetActive(true);
        gachaAnim.SetActive(false);

        UnitPieceTextUpdate();

        if (go != null)
            go.SetActive(false);
    }

    public void UnitPieceTextUpdate()
    {
        unitTabTierPiece[0].text = "B Ƽ��\n���� ��ȭ �ֹ��� " + pieceData.bPiece.ToString() + "��";
        unitTabTierPiece[1].text = "A Ƽ��\n���� ��ȭ �ֹ��� " + pieceData.aPiece.ToString() + "��";
        unitTabTierPiece[2].text = "S Ƽ��\n���� ��ȭ �ֹ��� " + pieceData.sPiece.ToString() + "��";
    }

























    //--------------------------------------------------------------------------------------Save

    public void Save(ref Save_UnitData data)
    {
        data.unitSaveDatas.Clear();
        data.unitSaveDatas = new();

        foreach (var (key, item) in unitDataDic)
        {
            UnitSaveData newData = new();
            item.Save(ref newData);

            data.unitSaveDatas.Add(newData);
        }

        PieceSaveData newPiece = new();
        pieceData.Save(ref newPiece);
        data.pieceData = newPiece;
    }

    //Start���� ��
    public void Load(Save_UnitData data)
    {
        foreach (var item in data.unitSaveDatas)
        {
            UnitData unit = new();
            unit.Init(unitLoader.GetByKey(item.Key), upgradeLoader.GetByKey(unitLoader.GetByKey(item.Key).UpgradeKey));
            unit.Load(item, upgradeLoader.GetByKey(unitLoader.GetByKey(item.Key).UpgradeKey));

            unitDataDic.Add(item.Key, unit);
        }

        pieceData.Load(data.pieceData);
    }
}
