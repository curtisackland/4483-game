using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Bar")]
    private float health;

    public float maxHealth = 100;

    public float chipSpeed = 2f;

    private float lerpTimer;

    public Image frontHealthBar;
    
    public Image backHealthBar;

    public TextMeshProUGUI healthAmountText;

    public TextMeshProUGUI deathText;

    [Header("Damage Overlay")]
    public Image overlay;

    public AudioSource takeDamageSound;

    public float duration;

    public float fadeSpeed;

    private float durationTimer;
    
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }
    
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        healthAmountText.text = health.ToString();
        
        UpdateHealthUI();
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;
        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; // makes animation look smoother
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        } else if (fillFront < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete; // makes animation look smoother
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.5f);
        takeDamageSound.Play();
        
        if (health <= 0)
        {
            StartCoroutine(RestartGame(3f));
        }
    }

    private IEnumerator RestartGame(float restartTime)
    {
        deathText.text = "You died! Restarting...";
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(restartTime);
        Time.timeScale = 1;
        health = maxHealth;
        deathText.text = String.Empty;
        transform.SetPositionAndRotation(new Vector3(496.8852f, 1.175f, 524.1196f), new Quaternion(0, 180, 0, 1));
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
