using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    [Header("XP Bar")]
    private float xp = 0;
    
    public float xpNeededForLevel = 100;

    public float chipSpeed = 2f;

    private float lerpTimer;

    public Image frontXPBar;
    
    public Image backXPBar;

    public TextMeshProUGUI xpLevelText;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        //xp = Mathf.Clamp(xp, 0, maxHealth);
        UpdateXPUI();
    }

    public void UpdateXPUI()
    {
        xpLevelText.text = Math.Floor(xp / xpNeededForLevel).ToString();
        float fillFront = frontXPBar.fillAmount;
        float fillBack = backXPBar.fillAmount;
        float healthFraction = xp % xpNeededForLevel / xpNeededForLevel;
        
        /*
        if (fillBack > healthFraction)
        {
            frontXPBar.fillAmount = healthFraction;
            backXPBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; // makes animation look smoother
            backXPBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        } else if (fillFront < healthFraction)
        {
            backXPBar.fillAmount = healthFraction;
            backXPBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; // makes animation look smoother
            frontXPBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }
        */
        
        backXPBar.fillAmount = healthFraction;
        backXPBar.color = Color.green;
        lerpTimer += Time.deltaTime;
        float percentComplete = lerpTimer / chipSpeed;
        percentComplete = percentComplete * percentComplete; // makes animation look smoother
        frontXPBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
    }

    public void AddXP(float xpAmount)
    {
        xp += xpAmount;
        lerpTimer = 0f;
    }
}
