using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public Slider healthbar;
    public TextMeshProUGUI bossName; 
    public GameObject player;
    [SerializeField] public List<Enemy> bosses;
    [SerializeField] public List<float> bossbarDistance;
    public AudioSource bossMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowBossbar(false);
    }

    // Update is called once per frame
    void Update()
    {
        int closestBoss = -1;
        float closestBossDistance = float.MaxValue;
        for (int i = 0; i < bosses.Count; i++)
        {
            if (!bosses[i].IsDestroyed() && bosses[i].isActiveAndEnabled)
            {
                var bossDistance = (player.transform.position - bosses[i].transform.position).magnitude;
                if (bossDistance < closestBossDistance)
                {
                    closestBoss = i;
                    closestBossDistance = bossDistance;
                }
            }
        }

        if (closestBoss != -1)
        {
            var bossDistance = (player.transform.position - bosses[closestBoss].transform.position).magnitude;
            if (bossDistance < bossbarDistance[closestBoss])
            {
                ShowBossbar(true);
                healthbar.value = bosses[closestBoss].health / bosses[closestBoss].maxHealth;
                bossName.text = bosses[closestBoss].enemyName;
            }
            else
            {
                ShowBossbar(false);
            }
        }
        else
        {
            ShowBossbar(false);
        }
    }

    void ShowBossbar(bool show)
    {
        healthbar.gameObject.SetActive(show);
        bossName.gameObject.SetActive(show);
        bossMusic.enabled = show;
    }
}
