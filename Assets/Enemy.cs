using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float yTolerance = 0.1f; // Y座標の許容範囲
    public float chaseRange = 10f;  // 敵が追尾を開始できる範囲

    public Vector2 moveAreaMin = new Vector2(-10, -10); // 移動範囲の最小X,Z
    public Vector2 moveAreaMax = new Vector2(10, 10);   // 移動範囲の最大X,Z

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
            // プレイヤーのY高さが近く、範囲内なら追尾
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Vector3 targetPos = player.position;
            targetPos.y = transform.position.y; // 高さは変えない

            // 範囲チェック
            if (IsWithinMoveArea(targetPos))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            // 初期位置へ戻る
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