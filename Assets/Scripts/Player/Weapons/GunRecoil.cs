using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{

    public GunData gunData;
    
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    
    private float currentRecoilPosition, finalRecoilPosition;

    private Transform camHolder;

    private ScopeIn scopeIn;
    
    void Start()
    {
        camHolder = transform.parent.parent.parent.transform;
        scopeIn = GetComponent<ScopeIn>();
    }

    // Update is called once per frame
    void Update()
    {
        // bullet recoil
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gunData.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gunData.snappiness * Time.fixedDeltaTime);
        camHolder.SetLocalPositionAndRotation(camHolder.localPosition, Quaternion.Euler(currentRotation));
        
        // gun recoil
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, gunData.gunReturnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, gunData.kickBackSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, finalRecoilPosition);
    }

    public void RecoilFire()
    {
        currentRecoilPosition += gunData.kickBackAmount;
        
        // ADS and hipfire have different recoils
        if (scopeIn.isADS())
        {
            targetRotation += new Vector3(gunData.adsRecoilX, Random.Range(-gunData.adsRecoilY, gunData.adsRecoilY), Random.Range(-gunData.adsRecoilZ, gunData.adsRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(gunData.hipFireRecoilX, Random.Range(-gunData.hipFireRecoilY, gunData.hipFireRecoilY), Random.Range(-gunData.hipFireRecoilZ, gunData.hipFireRecoilZ));
        }
        
    }
}
