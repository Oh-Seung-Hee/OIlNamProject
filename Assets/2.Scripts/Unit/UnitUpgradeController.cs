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
    public GameObject slots;
    private UnitUpgradeSlot[] upgradeSlots;
    private Dictionary<int, UnitData> slotDic;

    [Header("UpgradeUI")]
    public UnitUpgradeUI unitUpgradeUI;


    void Start()
    {
        unitManager = GameManager.Instance.UnitManager;
        upgradeLoader = GameManager.Instance.DataManager.dataTable_UpgradeLoader;

        upgradeSlots = slots.GetComponentsInChildren<UnitUpgradeSlot>();

        if (upgradeSlots.Length == 0)
            Debug.Log("getcomponenets ���ϴ���");
    }

    //��ȭ�� �̵��Ҷ�//BTN
    public void OnUnitUpgradeTab()
    {
        GameManager.Instance.SoundManager.EffectAudioClipPlay((int)EffectList.Intro);

        NormalSetUpgradeSlots();

        slots.SetActive(true);
    }

    private void NormalSetUpgradeSlots()
    {
        slotDic.Clear();

        int index = 0;

        foreach (var (key, item) in unitManager.unitDataDic)
        {
            slotDic.Add(index, item);
            upgradeSlots[index].Init(item, upgradeLoader.GetByKey(key));

            index++;
        }
    }

    //��ȭâ �Ӷ�//BTN
    public void OnUnitTab(int slotNum)//���Կ� �޾Ƽ� ��ưŬ���Ҷ� �����θ� �ֱ�
    {
        //�Ű������� go�� �޾Ƽ�
        //UnitUpgradeSlot slot = go.GetComponentInChildren<UnitUpgradeSlot>();

        unitUpgradeUI.Init(this, slotNum, slotDic[slotNum], upgradeLoader.GetByKey(slotDic[slotNum].key));
        unitUpgradeUI.gameObject.SetActive(true);
    }

    //�����ؽ�Ʈ ������Ʈ
    public void UpdateSlot(int slotNum)
    {
        //�����ٲ��ȵ�
        slotDic[slotNum].Upgrade();
        upgradeSlots[slotNum].UpdateText();
    }
}
