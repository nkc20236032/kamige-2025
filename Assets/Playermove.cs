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

    public List<GameObject> destroyTargets = new List<GameObject>(); // �o�^���X�g
    public float destroyDistance = 3.0f; // �󂹂鋗��
    public KeyCode destroyKey = KeyCode.E; // �󂷃L�[

    public Vector3 warpDestination;


    void Update()
    {
        // �ړ����́F�����Ă�����ɑO�i�E���
        float moveInput = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += transform.forward * moveInput;

        // ��]���́F���E����
        float rotationInput = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationInput, 0f);

        // �͈͐���
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);
        transform.position = clampedPosition;

        // �J�����Ǐ]�F�I�u�W�F�N�g�̉�]�ɉ����Ĉʒu����]
        if (cameraTransform != null)
        {
            Vector3 desiredPosition = transform.position + transform.rotation * cameraOffset;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

            // �I�u�W�F�N�g�̉�]�ɃJ���������킹��
            Quaternion desiredRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, desiredRotation, cameraSmoothSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(destroyKey))
        {
            GameObject nearest = FindNearestTarget();
            if (nearest != null)
            {
                destroyTargets.Remove(nearest); // ���X�g����폜
                Destroy(nearest);               // �I�u�W�F�N�g�j��
                Debug.Log($"{nearest.name} ��j�󂵂܂����I");
            }
        }
    }

    GameObject FindNearestTarget()
    {
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject target in destroyTargets)
        {
            if (target == null) continue; // ���ɔj�󂳂�Ă���̂̓X�L�b�v

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
