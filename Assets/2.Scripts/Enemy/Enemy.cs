using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyMove enemyMove;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� Ȱ��ȭ
    private void Acticate()
    {

    }

    // ���� ��Ȱ��ȭ
    private void Disable()
    {

    }
}
