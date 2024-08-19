using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance;
    public CinemachineVirtualCamera Overview;
    public CinemachineVirtualCamera FirstPersonCamera;
    public Vector3 _position;
    public Quaternion _rotation;
    private void Awake()
    {
        Instance = this;
        _position= FirstPersonCamera.transform.position;
        _rotation=FirstPersonCamera.transform.rotation;
        FirstPersonCamera.gameObject.SetActive(false);
    }

    public void ToggleFirstPerson(bool toggle) => FirstPersonCamera.gameObject.SetActive(toggle);
    public void ResetFirstPerson()
    {
        //Transform tr = FirstPersonCamera.LookAt;
        //FirstPersonCamera.LookAt = null;
        //FirstPersonCamera.transform.position = _position;
        //FirstPersonCamera.transform.rotation = _rotation;

        //FirstPersonCamera.LookAt = tr;
    }

}
