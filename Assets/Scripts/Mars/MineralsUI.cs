using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MineralsUI : MonoBehaviour
{
    [SerializeField] private Scrollbar MineralsProgressBar; 
    public int collectedMinerals = 0; 
    private int totalMinerals = 20;     

    void Start()
    {
        if (MineralsProgressBar != null)
        {
            MineralsProgressBar.size = 0;
        }
    }

    public void AddMinerals(int MineralNum)
    {
        if (collectedMinerals+MineralNum < totalMinerals)
        {
            collectedMinerals+=MineralNum;
            UpdateMineralsProgress();
            Debug.Log($"FMinerals Progress: {collectedMinerals}/{totalMinerals}");
        }
        else{
            collectedMinerals = totalMinerals;
            UpdateMineralsProgress();
        }
    }

    private void UpdateMineralsProgress()
    {
        if (MineralsProgressBar != null)
        {
            MineralsProgressBar.size = (float)collectedMinerals / totalMinerals;
        }
    }
    public bool isDone()
    {
        return collectedMinerals==totalMinerals;
    }
}
