using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class Objectives : MonoBehaviour
{

    public PlayerXP playerStats;
    
    private TextMeshProUGUI objectiveText;

    private string[] objectives;

    private int completedObjectives = 0;
    void Start()
    {
        objectiveText = GetComponent<TextMeshProUGUI>();
        objectives = new string[7];
        objectives[0] = "Reach XP level 10";
        objectives[1] = "Kill the boss in the North of Area 52";
        objectives[2] = "Reach XP level 20";
        objectives[3] = "Kill the boss in the East of Area 52";
        objectives[4] = "Reach XP level 30";
        objectives[5] = "Kill the boss in the South of Area 52";
        objectives[6] = "Win the game!";
    }
    
    void Update()
    {
        objectiveText.text = objectives[completedObjectives];
        if (completedObjectives == 0 && playerStats.GetXP() >= 10)
        {
            completedObjectives++;
        } else if (completedObjectives == 1)
        {
            // TODO insert boss completion here
            completedObjectives++;
        } else if (completedObjectives == 2 && playerStats.GetXP() >= 20)
        {
            completedObjectives++;
        } else if (completedObjectives == 3)
        {
            // TODO insert boss completion here
            completedObjectives++;
        } else if (completedObjectives == 4 && playerStats.GetXP() >= 30)
        {
            completedObjectives++;
        } else if (completedObjectives == 5)
        {
            // TODO insert boss completion here
            completedObjectives++;
        }
    }
}
