using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float yTolerance = 0.1f; // Y���W�̋��e�͈�
    public float chaseRange = 10f;  // �G���ǔ����J�n�ł���͈�

    public Vector2 moveAreaMin = new Vector2(-10, -10); // �ړ��͈͂̍ŏ�X,Z
    public Vector2 moveAreaMax = new Vector2(10, 10);   // �ړ��͈͂̍ő�X,Z

    private Vector3 startPos;
    private bool isChasing = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yDifference = Mathf.Abs(transform.position.y - player.position.y);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (yDifference <= yTolerance && distanceToPlayer <= chaseRange)
        {
            // �v���C���[��Y�������߂��A�͈͓��Ȃ�ǔ�
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Vector3 targetPos = player.position;
            targetPos.y = transform.position.y; // �����͕ς��Ȃ�

            // �͈̓`�F�b�N
            if (IsWithinMoveArea(targetPos))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            // �����ʒu�֖߂�
            if (transform.position != startPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            }
        }
    }

    bool IsWithinMoveArea(Vector3 pos)
    {
        return pos.x >= moveAreaMin.x && pos.x <= moveAreaMax.x &&
               pos.z >= moveAreaMin.y && pos.z <= moveAreaMax.y;
    }
}