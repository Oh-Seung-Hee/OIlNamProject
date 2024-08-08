using Constants;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitUpgradeController : MonoBehaviour
{
    private UnitManager unitManager;
    private DataTable_UpgradeLoader upgradeLoader;

    [Header("UpgradeSlot")]
    public GameObject slots;//켜져있어야함
    private UnitUpgradeSlot[] upgradeSlots;
    private Dictionary<int, UnitData> slotDic = new();

    [Header("UpgradeUI")]
    public UnitUpgradeUI unitUpgradeUI;


    void Start()
    {
        unitManager = GameManager.Instance.UnitManager;
        upgradeLoader = GameManager.Instance.DataManager.dataTable_UpgradeLoader;

        if(slots != null )
            upgradeSlots = slots.GetComponentsInChildren<UnitUpgradeSlot>();

        if (upgradeSlots == null || upgradeSlots.Length == 0)
            Debug.Log("getcomponenets 못하는중");
    }

    //강화탭 이동할때//BTN
    public void OnUnitTab()
    {
        GameManager.Instance.SoundManager.EffectAudioClipPlay((int)EffectList.Intro);

        NormalSetUpgradeSlots();
    }

    private void NormalSetUpgradeSlots()
    {
        slotDic.Clear();

        int index = 0;

        foreach (var (key, item) in unitManager.unitDataDic)
        {
            slotDic.Add(index, item);
            upgradeSlots[index].Init(item, upgradeLoader.GetByKey(item.upgradeKey));

            index++;
        }

        unitManager.UnitPieceTextUpdate();
    }

    //강화창 켤때//BTN
    public void OnUpgradeUI(GameObject go)//슬롯에 달아서 버튼클릭할때 스스로를 주기
    {
        int slotNum = int.Parse(go.name.ToString());

        //매개변수에 go를 받아서
        //UnitUpgradeSlot slot = go.GetComponentInChildren<UnitUpgradeSlot>();

        unitUpgradeUI.Init(this, slotNum, slotDic[slotNum], upgradeLoader.GetByKey(slotDic[slotNum].upgradeKey));
        unitUpgradeUI.gameObject.SetActive(true);
    }

    //슬롯 업데이트
    public void UpdateSlot(int tier,int slotNum)
    {
        //순서바뀌면안됨 1.슬롯속 유닛값바뀌기 2.슬롯텍스트바뀌기
        unitManager.UsePiece((PieceType)tier, slotDic[slotNum].Upgrade());//나중엔 unitManager.UsePiece 필요없을지도
        //^ 티어별 피스 바꾸기 , 몇개의 needPiece인지 반환, 피스바꾸기

        foreach(var slot in upgradeSlots)
        {
            if (slot.myUnitData.tier == tier)
                slot.UpdateText();
        }
    }

    //가챠BTN
    public void GachaUnitPieceBtn(int count)
    {
        unitManager.GachaUnitPiece(count);
    }
}
