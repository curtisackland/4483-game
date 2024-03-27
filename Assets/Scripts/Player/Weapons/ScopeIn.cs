using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ScopeIn : MonoBehaviour
{

    [Header("Scope Position and Zoom")]
    public Transform scopePos;

    public Transform gunPos;

    public Transform gun;

    private Camera cam;

    public float zoomAmount = 1f;

    private float defaultFov;

    public float adsTime = 1f;

    private bool isPlayerADS = false;
    
    // Start is called before the first frame update
    void Start()
    {
        UIWeaponData temp = GetComponentInParent<UIWeaponData>();
        cam = temp.mainCamera;
        defaultFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isPlayerADS = true;
            gun.position = Vector3.Lerp(gun.position, scopePos.position, 1/adsTime * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov/zoomAmount, 1/adsTime * Time.deltaTime);
        }
        else
        {
            isPlayerADS = false;
            gun.position = Vector3.Lerp(gun.position, gunPos.position, 1/adsTime * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, 1/adsTime * Time.deltaTime);
        }
    }

    public bool isADS()
    {
        return isPlayerADS;
    }
}
