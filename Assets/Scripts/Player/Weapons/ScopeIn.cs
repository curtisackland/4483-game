using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScopeIn : MonoBehaviour
{

    [Header("Scope Position and Zoom")]
    public Transform scopePos;

    public Transform gunPos;

    public Transform gun;

    private MeshRenderer gunRenderer;

    private Camera cam;

    public float zoomAmount = 1f;

    private float defaultFov;

    public float adsTime = 1f;

    private bool isPlayerADS = false;

    private Image scopeImage;

    private string gunName;

    private float currentAlpha;
    
    // Start is called before the first frame update
    void Start()
    {
        UIWeaponData temp = GetComponentInParent<UIWeaponData>();
        cam = temp.mainCamera;
        scopeImage = temp.scopeOverlay;
        scopeImage.color = new Color(scopeImage.color.r, scopeImage.color.b, scopeImage.color.g, 0);
        gunName = GetComponent<Gun>().gunData.name;
        defaultFov = cam.fieldOfView;
        gunRenderer = gun.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isPlayerADS = true;
            gun.position = Vector3.Lerp(gun.position, scopePos.position, 1/adsTime * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov/zoomAmount, 1/adsTime * Time.deltaTime);
            if (gunName == "Sniper")
            {
                currentAlpha = Mathf.Lerp(currentAlpha, 1, 1 / adsTime * Time.deltaTime);
                scopeImage.color = new Color(scopeImage.color.r, scopeImage.color.b, scopeImage.color.g, currentAlpha);
                gunRenderer.enabled = false;
            }
        }
        else
        {
            isPlayerADS = false;
            gun.position = Vector3.Lerp(gun.position, gunPos.position, 1/adsTime * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, 1/adsTime * Time.deltaTime);
            if (gunName == "Sniper")
            {
                currentAlpha = Mathf.Lerp(currentAlpha, 0, 1/adsTime * Time.deltaTime);
                scopeImage.color = new Color(scopeImage.color.r, scopeImage.color.b, scopeImage.color.g, currentAlpha);
                gunRenderer.enabled = true;
            }
        }
    }

    public bool isADS()
    {
        return isPlayerADS;
    }
}
