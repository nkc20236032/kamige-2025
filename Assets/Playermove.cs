using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public Vector3 minBounds = new Vector3(-10f, 0f, -10f);
    public Vector3 maxBounds = new Vector3(10f, 0f, 10f);

    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 3f, -6f);
    public float cameraSmoothSpeed = 5f;

    public List<GameObject> destroyTargets = new List<GameObject>(); // 登録リスト
    public float destroyDistance = 3.0f; // 壊せる距離
    public KeyCode destroyKey = KeyCode.E; // 壊すキー

    public Vector3 warpDestination;


    void Update()
    {
        // 移動入力：向いてる方向に前進・後退
        float moveInput = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += transform.forward * moveInput;

        // 回転入力：左右旋回
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationInput, 0f);

        // 範囲制限
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);
        transform.position = clampedPosition;

        // カメラ追従：オブジェクトの回転に沿って位置も回転
        if (cameraTransform != null)
        {
            Vector3 desiredPosition = transform.position + transform.rotation * cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

            // オブジェクトの回転にカメラも合わせる
            Quaternion desiredRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, desiredRotation, cameraSmoothSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(destroyKey))
        {
            GameObject nearest = FindNearestTarget();
            if (nearest != null)
            {
                destroyTargets.Remove(nearest); // リストから削除
                Destroy(nearest);               // オブジェクト破壊
                Debug.Log($"{nearest.name} を破壊しました！");
            }
        }
    }

    GameObject FindNearestTarget()
    {
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in destroyTargets)
        {
            if (target == null) continue; // 既に破壊されてるものはスキップ

            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= destroyDistance && distance < minDistance)
            {
                minDistance = distance;
                nearest = target;
            }
        }
        return nearest;
    }
    
    }
