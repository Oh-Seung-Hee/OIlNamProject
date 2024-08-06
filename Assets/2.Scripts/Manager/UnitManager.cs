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

[Serializable]
public struct Save_UnitData
{
    public List<UnitSaveData> unitSaveDatas;
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
    public string name;
    public int tier;
    public int atk;
    public int speed;
    public int range;
    public bool open;

    //��ȭ������
    public int level;
    public int piece;

    //Sprite
    public Sprite sprite;

    //���׷��̵� ����
    private DataTable_Upgrade upgradeData;

    public void Init(DataTable_Unit unit)
    {
        key = unit.key;
        name = unit.Name;
        tier = unit.Tier;
        atk = unit.ATK;
        speed = unit.Speed;
        range = unit.Range;
        open = unit.Open;

        level = 0;
        piece = 0;

        sprite = Resources.Load<Sprite>("Unit/" + unit.Path);
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

    public void Upgrade()
    {
        piece -= upgradeData.NeedPiece[level];
        atk += upgradeData.ATK[level];
        speed += upgradeData.Speed[level];
        range += upgradeData.Range[level];
        level++;
    }
}

public class UnitManager : Manager
{
    [Header("Temp")]//Data
    public DataManager DataManager;
    private DataTable_UnitLoader unitLoader; // ��������
    private DataTable_UpgradeLoader upgradeLoader; //��������

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
        if (GameManager.Instance == null)
        {
            unitLoader = DataManager.dataTable_UnitLoader;
            upgradeLoader = DataManager.dataTable_UpgradeLoader;
        }

        if (unitDataDic == null)
        {
            FirstInit();
        }

        InitTierID();
    }

    private void FirstInit()
    {
        foreach (var item in unitLoader.ItemsList)
        {
            UnitData newData = new();
            newData.Init(item);

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
    }

    //Start���� ��
    public void Load(Save_UnitData data)
    {
        foreach (var item in data.unitSaveDatas)
        {
            UnitData unit = new();
            unit.Init(unitLoader.GetByKey(item.Key));
            unit.Load(item, upgradeLoader.GetByKey(item.Key));

            unitDataDic.Add(item.Key, unit);
        }
    }
}
