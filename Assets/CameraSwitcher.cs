using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    public MonoBehaviour fpsCameraScript; // FPSカメラスクリプト（例：FPSCamera.cs）
    public int fpsCameraIndex = 1; // FPSカメラがcameras配列の何番目か
    public Transform playerBody;
    private int currentCameraIndex = 0;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            bool isActive = (i == currentCameraIndex);
            cameras[i].gameObject.SetActive(isActive);
        }

        // 初期化時のFPSカメラスクリプトのON/OFF
        if (fpsCameraScript != null)
            fpsCameraScript.enabled = (currentCameraIndex == fpsCameraIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(2);
    }

    void SwitchCamera(int index)
    {
        if (index < 0 || index >= cameras.Length) return;

        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[index].gameObject.SetActive(true);

        // FPSスクリプトのON/OFF切り替え
        if (fpsCameraScript != null)
        {
            fpsCameraScript.enabled = (index == fpsCameraIndex);
        }

        currentCameraIndex = index;
    }
}