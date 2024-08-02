using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnitData
{
    public int id;
    public float range;
    public float speed;
    public int step;

    public float fixSpeedData;//�ٲ���x
    public float fixSpeed;
    public float currentSpeed;

    public float fixAtk;
    public float currentAtk;

    public void Init(int id, float range, float speed, float atk, int step = 0)
    {
        this.id = id;
        this.range = range;
        fixSpeedData = speed;
        this.speed = speed;
        fixAtk = atk;

        if (step == 0)
            step = 0;
        this.step += step;

        currentAtk = fixAtk;
    }

    public void SpeedChange(int changeVal, bool isFixChange = false)//value�� �� ���� �ۼ�Ʈ
    {
        if (changeVal == 0)
        {
            if (isFixChange)
                fixSpeed = fixSpeedData;
            else
                speed = fixSpeed;

            return;
        }

        if (isFixChange)
            fixSpeed = fixSpeedData + fixSpeedData / 100 * changeVal;
        else
            speed = fixSpeed + fixSpeed / 100 * changeVal;
    }

    public void ATKChange(int changeVal, bool isFixChange = false)
    {
        if (changeVal == 0)
            return;

        if (isFixChange)
            fixAtk += fixAtk / 100 * changeVal;
        else
            currentAtk += currentAtk / 100 * changeVal;
    }

}

public class Unit : MonoBehaviour, IPointerClickHandler
{
    public UnitController controller; // ���߿�private
    private UnitAnimation unitAnimation;

    [Header("Upgrade")]
    public GameObject btnUI;
    public Image nonClickImage;
    public List<Image> iconImage;

    [Header("Self")]
    public GameObject skillGO;
    public GameObject rangeGO;

    public UnitData myData;
    /*    public int id;
        public float range = 0;
        public float speed = 0;
        public float time = 0;
        private int step = 0;*/
    private CircleCollider2D rangeCollider;

    private List<Enemy> enemyList = new();
    private Enemy findEnemy;


    private void Start()
    {
        if (GameManager.Instance != null)
            controller = GameManager.Instance.UnitController;

        unitAnimation = GetComponent<UnitAnimation>();

        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.radius = myData.range;
        rangeGO.transform.localScale = new Vector3(myData.range, myData.range);
    }

    private void Update()
    {
        myData.speed += Time.deltaTime;


        if (myData.speed >= myData.currentSpeed)
        {

            findEnemy = FindEnemy();
            if (findEnemy != null)
            {
                Debug.Log("��");

                unitAnimation.AttackEffect();
                myData.speed = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.onUnitPopUP.Count != 0)
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

        UIOnOff();
    }

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
    }

    public void UIOff()
    {
        controller.onUnitPopUP[0].SetActive(false);
        controller.onUnitPopUP[1].SetActive(false);

        controller.onUnitPopUP.Clear();
        /*        btnUI.gameObject.SetActive(false);
                rangeGO.SetActive(false);*/
    }

    public void UnitUpgrade()
    {
        controller.UnitUpgrade(myData.id, transform.position);

        if (myData.step - 2 >= 0)
            iconImage[myData.step - 2].gameObject.SetActive(false);

        iconImage[myData.step - 1].gameObject.SetActive(true);
    }

    public void SellUnit()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�浹");

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

    public void Attack()//animation���� ȣ���ϱ�
    {
        if (findEnemy == null)
            return;

        skillGO.transform.position = findEnemy.transform.position;
        unitAnimation.AttackSkillEffect();//Ÿ�̹��ذ��Ҽ������� ���ݳ����� ȣ��
        Debug.Log("Attackȣ���");
        findEnemy.EnemyAttacked(myData.currentAtk);
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
}
