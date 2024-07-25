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

    private DataTable_Enemy enemy;
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
    public void Init(int id, GameSceneManager GSM, DataManager DM = null/*, DataTable_EnemyLoader enemyDatabase*/)//����
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
            dataManager = gameManager.DataManager;
            enemyDatabase = dataManager.dataTable_EnemyLoader;
            gameSceneManager = GSM;//����
        }
        else
        {
            dataManager = DM;
            gameSceneManager = GSM;//����
            enemyDatabase = dataManager.dataTable_EnemyLoader;
        }

        // Script
        enemyMove = GetComponent<EnemyMove>();
        //gameManager = GameManager.Instance;
        //dataManager = gameManager.DataManager;
        //this.enemyDatabase = dataManager.dataTable_EnemyLoader;

        // 
        enemy = enemyDatabase.GetEnemyByKey(id);
        image = GetComponent<SpriteRenderer>();
        image.sprite = enemy.sprite;
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
        float hp = enemy.HP;
        hp -= damage;

        // ���� �׾��� ��
        if (hp <= 0)
        {
            hp = 0;

            Disable();
        }
    }
}
