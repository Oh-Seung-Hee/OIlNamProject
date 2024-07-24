using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

[System.Serializable]
public class SpawnData
{
    public GameObject unitGo;
    public Transform transform;
    public UnitInstance unitInstance;
    public int upgradeStep;

    public void Init(GameObject go, UnitInstance unit)
    {
        unitGo = go;
        transform = go.GetComponent<Transform>();
        unitInstance = unit;
        upgradeStep = 1;

        go.GetComponentInChildren<SpriteRenderer>().sprite = unitInstance.unitInfo.Sprite;
        go.GetComponentInChildren<Unit>().id = unit.id;
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

    [Header("Temp")]
    public DataManager DataManager;
    private UnitDataBase unitDataBase;

    [Header("UI")]
    public TMP_Text infoTxt;

    public Dictionary<Vector3,SpawnData> spawnData = new();
    public Dictionary<int, int> canUpgrade = new();

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            unitDataBase = GameManager.Instance.DataManager.unitDataBase;
            unitManager = GameManager.Instance.UnitManager;
            GameManager.Instance.UnitController = this;
        }
        else
        {
            unitDataBase = DataManager.unitDataBase;
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

        UnitInstance newUnit = unitManager.GetRandomUnit();

        SpawnData newData = new SpawnData();
        newData.Init(unitGo, newUnit);
        
        //�ӽ�
        unitGo.GetComponentInChildren<Unit>().controller = this;

        infoTxt.text = newUnit.unitInfo.Name + " ���Ḧ ȹ���Ͽ����ϴ�.";

        spawnData.Add(canSpawn.pos,newData);

        if (canUpgrade.ContainsKey(newUnit.id) == true)
            canUpgrade[newUnit.id]++;
        else
            canUpgrade.Add(newUnit.id, 1);
    }

    public bool CanUpgradeCheck(int id)
    {
        if (canUpgrade[id] >= 3)
            return true;
        else
            return false;
    }

    public void UnitUpgrade()
    {

    }
}
