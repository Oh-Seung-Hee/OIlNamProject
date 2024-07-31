using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Script
    public EnemyMove enemyMove;
    private GameManager gameManager;
    private DataManager dataManager;
    private DataTable_EnemyLoader enemyDatabase;
    private GameSceneManager gameSceneManager;//����
    private EnemySpawn enemySpawn;    

    // ���� ����
    public DataTable_Enemy enemyData;
    private SpriteRenderer image;
    private bool isDead;

    //private void Start()
    //{
    //    if (GameManager.Instance != null)
    //    {
    //        enemyDatabase = GameManager.Instance.DataManager.dataTable_EnemyLoader;
    //    }
    //    else
    //    {
    //        enemyDatabase = dataManager.dataTable_EnemyLoader;
    //    }
    //}

    // ���� ���� �ʱ�ȭ
    public void Init(int id, GameSceneManager GSM, DataManager DM = null)//����
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
            dataManager = gameManager.DataManager;
            enemyDatabase = dataManager.dataTable_EnemyLoader;
            enemySpawn = gameManager.EnemySpawn;
            gameSceneManager = GSM;//����
        }
        else
        {
            dataManager = DM;
            gameSceneManager = GSM;//����
            enemyDatabase = dataManager.dataTable_EnemyLoader;
            enemySpawn = gameManager.EnemySpawn;
        }

        // Script
        enemyMove = GetComponent<EnemyMove>();
        //gameManager = GameManager.Instance;
        //dataManager = gameManager.DataManager;
        //this.enemyDatabase = dataManager.dataTable_EnemyLoader;

        // 
        enemyData = enemyDatabase.GetEnemyByKey(id);
        image = GetComponent<SpriteRenderer>();
        image.sprite = enemyData.sprite;
    }

    // ���� Ȱ��ȭ
    private void Activate()
    {
        //Init();
        isDead = false;
        this.gameObject.SetActive(true);
    }

    // ���� ��Ȱ��ȭ
    private void Disable()
    {
        isDead = true;
        this.gameObject.SetActive(false);
        gameSceneManager.ChangeGold(2);//����
    }

    // ������ ���� �޾��� ��
    public void EnemyAttacked(float damage)
    {
        float hp = enemyData.HP;
        hp -= damage;

        // ���� �׾��� ��
        if (hp <= 0)
        {
            hp = 0;

            //Disable();
            //*** TODO : (�ӽù������� ������Ʈ ���� ����->) �Ŀ� �Ʒ� �ڵ�� ��Ȱ��ȭ�ϰ� �� �� ���� Disable() Ȱ��ȭ ***
            gameSceneManager.ChangeGold(2);//����
            enemySpawn.EnemyDie(this, gameObject);
        }
    }
}