using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitGameData : MonoBehaviour, IPointerClickHandler
{
    private UnitSpawnController controller;
    private UnitAnimation unitAnimation;

    [Header("RangeCollider")]
    public CircleCollider2D rangeCollider;//���������θ� �־��ֱ�
    public GameObject rangeGO;

    [Header("UI")]
    public GameObject unitCanvas;

    [Header("Self")]
    public GameObject skillGO;


    private List<Enemy> enemyList = new();
    private Enemy findEnemy;

    //----------------------------------------------------------------Data

    private UnitData myUnitData;
    private DataTable_UnitStep myStepData;

    public float Range
    {
        get { return range; }
        private set
        {
            range = value;
            rangeCollider.radius = range;
        }
    }
    public float range;

    public int Step // if(Step >= 2)//0 = 1���϶�, 1 = 2���϶�
    {
        get { return step; }
        private set
        {
            step = value;
            SellGold = myStepData.SellGold[step];
        }
    }
    private int step;
    public int SellGold { get; private set; }

    //------------speed
    public float speedData;//���ο��� ��ȭ�� ���ְ��ݷ�--
    public float stepSpeed;//���Ǻ� ���ݷ��� ������ ���ݷ� (250% = 2.5)--

    public float fixSpeedStack; // �������ݷ� ������ (2% = 2)--
    public float fixSpeed; //�������ݷ½����� ���Ե� ���ݷ�

    public float speedStack; // ���� ���ݷ� ������ (40% = 40)--
    public float speed; //���� ���ݷ½����� ���Ե� ���ݷ�

    public float deltaSpeed;//delta���ؼ� speed�� ���ϴ¿�--

    //----------------atk
    public float atkData;//���ο��� ��ȭ�� ���ְ��ݷ�--
    public float stepAtk;//���Ǻ� ���ݷ��� ������ ���ݷ� (250% = 2.5)--

    public float fixAtkStack; // �������ݷ� ������ (2% = 2)--
    public float fixAtk; //�������ݷ½����� ���Ե� ���ݷ�

    public float atkStack; // ���� ���ݷ� ������ (40% = 40)--
    public float atk; //���� ���ݷ½����� ���Ե� ���ݷ�

    //------------------------------------------------------------------------

    public void Init(UnitData unitData, DataTable_UnitStep stepData, UnitSpawnController controller)
    {
        this.controller = controller;

        myUnitData = unitData;
        myStepData = stepData;

        //�빮�ڰ�����
        Range = unitData.range / 100;
        Step = 0;

        speedData = unitData.speed;
        fixSpeedStack = 0;
        speedStack = 0;
        SpeedChange();
        deltaSpeed = speed;

        atkData = unitData.atk;
        fixAtkStack = 0;
        atkStack = 0;
        ATKChange();

        unitAnimation = GetComponentInChildren<UnitAnimation>();
        unitAnimation.TypeSet(unitData.type);

        unitCanvas = transform.parent.GetChild(1).gameObject;
    }

    //���ǵ� ���� ���������� �θ��� = ����
    public void SpeedStackChange(int changeVal, bool isFixChange = false)
    {
        if (isFixChange)
            fixSpeedStack += changeVal;
        else
            speedStack += changeVal;

        SpeedChange();
    }
    public void ATKStackChange(int changeVal, bool isFixChange = false)
    {
        if (isFixChange)
            fixAtkStack += changeVal;
        else
            atkStack += changeVal;

        ATKChange();
    }

    //�ռ� ���׷��̵�
    public void UpgradeData()
    {
        step++;

        SpeedChange();
        ATKChange();

        //TODO : �����ܹٲٱ�
    }

    private void SpeedChange()
    {
        stepSpeed = speedData * (myStepData.StepSpeed[step] / 100f);
        fixSpeed = stepSpeed + ((stepSpeed / 100) * fixSpeedStack);
        speed = fixSpeed + ((fixSpeed / 100) * speedStack);
    }

    private void ATKChange()
    {
        stepAtk = atkData * (myStepData.StepATK[step] / 100f);
        fixAtk = stepAtk + ((stepAtk / 100) * fixAtkStack);
        atk = fixAtk + ((fixAtk / 100) * atkStack);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("���� �ν���");

        Enemy monster = collision.GetComponent<Enemy>();

        if (monster != null)
        {
            enemyList.Add(monster);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy monster = collision.GetComponent<Enemy>();

        if (monster != null)
            enemyList.Remove(monster);
    }

    private void Update()
    {
        deltaSpeed += Time.deltaTime;

        if (deltaSpeed >= speed)
        {
            findEnemy = FindEnemy();

            if (findEnemy != null)
            {
                Debug.Log("�Ҹ���");
                GameManager.Instance.SoundManager.GameAudioClipPlay(0);//���ݻ���
                unitAnimation.AttackEffect();//������ ���� ����Ʈ
                deltaSpeed = 0;
            }
        }
    }

    private Enemy FindEnemy()
    {
        if (enemyList.Count == 0)
            return null;
        if (enemyList.Count == 1)
            return enemyList[0];

        Enemy enemy = null;

        float min = float.MaxValue;
        float current = 0;

        foreach (Enemy monster in enemyList)
        {
            current = Vector3.Distance(transform.position, monster.transform.position);

            if (min > current)
            {
                enemy = monster;
                min = current;
            }
        }

        return enemy;
    }

    public void Attack()//animation���� ȣ���ϱ�
    {
        if (findEnemy == null)
            return;

        Debug.Log("Attackȣ���");

        //skillGO.transform.position = findEnemy.transform.position;
        //unitAnimation.AttackSkillEffect();//Ÿ�̹��ذ��Ҽ������� ���ݳ����� ȣ��
        findEnemy.EnemyAttacked(atk);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ŭ����");
        //controller.spriteCanvas.SetActive(true);

        gameObject.layer = 6;
        unitCanvas.SetActive(true);
        //������ 0

        /*        if (controller.onUnitPopUP.Count != 0)
                {
                    if (controller.onUnitPopUP[0] != btnUI)
                    {
                        controller.onUnitPopUP[0].SetActive(false);
                        controller.onUnitPopUP[1].SetActive(false);

                        controller.onUnitPopUP.Clear();

                        controller.onUnitPopUP.Add(btnUI);
                        controller.onUnitPopUP.Add(rangeGO);
                    }
                }
                else
                {
                    controller.onUnitPopUP.Add(btnUI);
                    controller.onUnitPopUP.Add(rangeGO);
                }

                UIOnOff();*/
    }
/*
    public void UIOnOff()
    {
        if (btnUI == null)
            return;

        if (controller.onUnitPopUP[0].activeSelf == true)
        {
            controller.onUnitPopUP[0].SetActive(false);
            controller.onUnitPopUP[1].SetActive(false);

            controller.onUnitPopUP.Clear();
        }
        else
        {
            if (controller.CanUpgradeCheck(myData.id) && myData.step < 2)
                nonClickImage.gameObject.SetActive(false);
            else
                nonClickImage.gameObject.SetActive(true);

            controller.onUnitPopUP[0].SetActive(true);
            controller.onUnitPopUP[1].SetActive(true);
        }

    mySprite.layer = 0;
    }

    public void UIOff()
    {
        controller.onUnitPopUP[0].SetActive(false);
        controller.onUnitPopUP[1].SetActive(false);

        controller.onUnitPopUP.Clear();
    mySprite.layer = 0;
        *//*        btnUI.gameObject.SetActive(false);
                rangeGO.SetActive(false);*//*
    }*/
}
