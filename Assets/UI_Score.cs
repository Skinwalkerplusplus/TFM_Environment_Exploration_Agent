using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;

public class UI_Score : MonoBehaviour
{
    public TextMeshProUGUI rewardText;
    public GeometryBugFinder agent;

    void Update()
    {
        rewardText.text = $"Total Reward: {agent.GetCumulativeReward():F2}";
    }
}
