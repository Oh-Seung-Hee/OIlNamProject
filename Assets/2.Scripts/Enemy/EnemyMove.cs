using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    private Vector3 direction;

    [SerializeField] private Transform[] wayPoints;
    private int wayPointCount;
    private int currentIndex;

    private void Awake()
    {

    }

    // ���� �ʱ�ȭ
    public void Init(Transform[] _wayPoints)
    {
        currentIndex = 0;

        // ��������Ʈ �Ҵ�
        wayPointCount = _wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = _wayPoints;

        this.transform.position = wayPoints[currentIndex].position;

        StartCoroutine(MoveWayPoint());
    }

    private void Update()
    {
        Move();
    }

    // ������ ������
    private void Move()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    // ������ ���� WayPoint
    private IEnumerator MoveWayPoint()
    {
        NextWayPoint();

        while (true)
        {
            if (Vector3.Distance(this.transform.position, wayPoints[currentIndex].position) < 0.05f * speed)
            {
                NextWayPoint();
            }

            yield return null;
        }
    }

    // ������ ���� WayPoint ����
    private void NextWayPoint()
    {
        // ���� wayPoint�� ���� ��
        if (currentIndex < wayPointCount - 1)
        {
            this.transform.position = wayPoints[currentIndex].position;
            currentIndex++;
        }
        // ������ wayPoint�� �������� ��
        else
        {
            this.transform.position = wayPoints[currentIndex].position;
            currentIndex = 0;
        }

        direction = (wayPoints[currentIndex].position - this.transform.position).normalized;
        MoveDirection(direction);
    }

    // ������ ������ ���� ����
    private void MoveDirection(Vector3 _direction)
    {
        direction = _direction;
    }
}
