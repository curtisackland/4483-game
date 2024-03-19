using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;

    [SerializeField] private float returnSpeed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
    }
}
