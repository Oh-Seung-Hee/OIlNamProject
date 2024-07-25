using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

[System.Serializable]
public class SpawnData
{
    public GameObject unitGo;
    public Unit unitData;
    public SpriteRenderer spriteRenderer;
    public Vector3 pos;
    public int id;
    public int upgradeStep;


    public void Init(GameObject go, UnitInstance unit, int upgrade = 1)
    {
        unitGo = go;
        pos = go.GetComponent<Transform>().position;
        id = unit.unitInfo.ID;
        upgradeStep = upgrade;

        spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = unit.unitInfo.Sprite;
        unitData = go.GetComponentInChildren<Unit>();
        unitData.myData.Init(unit.id, unit.unitInfo.Range, unit.unitInfo.Range, unit.unitInfo.ATK);
    }
}

public class CanUpgrade
{
    public int count = 0;
    public List<Vector3> pos = new();

    public void Init(int count, Vector3 pos)
    {
        this.count = count;
        this.pos.Add(pos);
    }
    public void AddCount(int count, Vector3 pos)
    {
        this.count += count;
        this.pos.Add(pos);
    }
}

public class UnitController : MonoBehaviour
{
    //unit = ��ȭ ����
    //controller = ����,ȣ��,�˻�
    // spawn = ����

    [Header("Unit")]
    public UnitSpawn unitSpawnGo;
    public UnitManager unitManager;// ���߿� private

    [Header("Prefab")]
    public GameObject UnitPrefab;

    [Header("UI")]
    public TMP_Text infoTxt;

    public Dictionary<Vector3, SpawnData> spawnData = new();
    public Dictionary<int, CanUpgrade> canUpgrade = new();

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            unitManager = GameManager.Instance.UnitManager;
            GameManager.Instance.UnitController = this;
        }
    }

    public void UnitRecall()
    {
        GameObject unitGo = Instantiate(UnitPrefab);
        CanSpawn canSpawn = unitSpawnGo.RandomUnitSpawn();

        if (canSpawn.canSpawn == true)
        {
            unitGo.transform.position = canSpawn.pos;
        }
        else
        {
            //TODO : ���Ḧ ���̻� ��ȯ�Ҽ������ϴ� �˾� Ȥ�� �۱� �ö󰡸鼭 ����������

            Debug.Log("���Ḧ ���̻� ��ȯ�Ҽ������ϴ�.");

            Destroy(unitGo);
            return;
        }

        UnitInstance newUnit = new();
        newUnit = unitManager.GetRandomUnit();

        SpawnData newData = new SpawnData();
        newData.Init(unitGo, newUnit);

        //�ӽ�
        unitGo.GetComponentInChildren<Unit>().controller = this;

        infoTxt.text = newUnit.unitInfo.Name + " ���Ḧ ȹ���Ͽ����ϴ�.";

        spawnData.Add(canSpawn.pos, newData);

        if (canUpgrade.ContainsKey(newUnit.id) == true)
        {
            canUpgrade[newUnit.id].count++;
            canUpgrade[newUnit.id].pos.Add(canSpawn.pos);
        }
        else
        {
            CanUpgrade temp = new CanUpgrade();
            temp.Init(1, canSpawn.pos);

            canUpgrade.Add(newUnit.id, temp);
        }
    }

    public bool CanUpgradeCheck(int id)
    {
        if (canUpgrade[id].count >= 3)
            return true;
        else
            return false;
    }

    public void UnitUpgrade(int id, Vector3 pos)
    {
        if (CanUpgradeCheck(id) == false)
            return;

        canUpgrade[id].count -= 3;
        canUpgrade[id].pos.Remove(pos);

        int count = 0;

        Vector3[] tempPos = new Vector3[2];

        foreach (var val in canUpgrade[id].pos)
        {
            if (val != pos && count < 2)
            {
                unitSpawnGo.PlusSpawnPoint(spawnData[val].pos);

                Destroy(spawnData[val].unitGo);
                spawnData.Remove(val);

                tempPos[count] = val;
                count++;
            }
        }

        canUpgrade[id].pos.Remove(tempPos[0]);
        canUpgrade[id].pos.Remove(tempPos[1]);

        spawnData[pos].upgradeStep++;
        spawnData[pos].id++;
        //spawnData[pos].spriteRenderer.sprite = unitDataBase.GetUnitByKey(id + 1).Sprite;----------���ݽ�������Ʈ�غ�ȴ�
        //�Ƹ� atk speed�� �ʿ��ҵ�


        //��������� ���� ����.
        /*        spawnData[pos].unitInstance.unitInfo = null;
                spawnData[pos].unitInstance.unitInfo = new();
                UnitInfo info = new();
                info = unitDataBase.GetUnitByKey(id + 1);
                spawnData[pos].unitInstance.unitInfo = info;*/



        if (canUpgrade.ContainsKey(id + 1) == true)
        {
            canUpgrade[id + 1].AddCount(1, pos);
        }
        else
        {
            CanUpgrade temp = new CanUpgrade();
            temp.Init(1, pos);

            canUpgrade.Add(id + 1, temp);
        }
    }
}
