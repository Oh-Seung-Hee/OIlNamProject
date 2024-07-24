using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    //unit = ��ȭ ����
    //controller = ����
    // spawn = ����

    [Header("Unit")]
    public UnitSpawn unitSpawnGo;
    private List<int> spawnUnit = new();

    [Header("Prefab")]
    public GameObject UnitPrefab;

    [Header("Temp")]
    public DataManager DataManager;
    private UnitDataBase unitDataBase;

    private void Start()
    {
        if (GameManager.Instance != null)
            unitDataBase = GameManager.Instance.DataManager.unitDataBase;
        else
            unitDataBase = DataManager.unitDataBase;
    }

    public void UnitRecall()
    {
        //������ ������
        

        //������
        GameObject unit = Instantiate(UnitPrefab);
        unit.transform.position = unitSpawnGo.RandomUnitSpawn();
        //spawnUnit.add
    }
}
