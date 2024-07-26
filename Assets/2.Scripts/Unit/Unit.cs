using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UnitData
{
    public int id;
    public float range;
    public float speed;
    public float time;
    public int step;
    public float atk;

    public void Init(int id, float range, float speed, float atk, int step = 0)
    {
        this.id = id;
        this.range = range;
        this.speed = speed;
        this.atk = atk;
        time = speed;

        if (step == 0)
            step = 0;
        this.step += step;
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

    [Header("Skill")]
    public GameObject skillGO;

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
    }

    private void Update()
    {
        myData.time += Time.deltaTime;


        if (myData.time >= myData.speed)
        {

            findEnemy = FindEnemy();
            if (findEnemy != null)
            {
                Debug.Log("��");

                unitAnimation.AttackEffect();
                myData.time = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myData.step == 3)
            return;

        UIOnOff(btnUI);
    }

    public void UIOnOff(GameObject ui)
    {
        if (ui == null)
            return;

        if (ui.activeSelf == true)
            ui.SetActive(false);
        else
        {
            if (controller.CanUpgradeCheck(myData.id))
                nonClickImage.gameObject.SetActive(false);
            else
                nonClickImage.gameObject.SetActive(true);

            Debug.Log(myData.id);
            Debug.Log(myData.step);
            Debug.Log(myData.atk);
            Debug.Log(myData.range);
            Debug.Log(myData.speed);

            ui.SetActive(true);
        }
    }

    public void UIOff()
    {
        btnUI.gameObject.SetActive(false);
    }

    public void UnitUpgrade()
    {
        controller.UnitUpgrade(myData.id, transform.position);
        iconImage[myData.step-1].gameObject.SetActive(true);
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
        unitAnimation.AttackSkillEffect();//Ÿ�̹��ذ��Ҽ������� ���ݳ����� ȣ��
        skillGO.transform.position = findEnemy.transform.position;
        Debug.Log("Attackȣ���");
        findEnemy.EnemyAttacked(myData.atk);
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
