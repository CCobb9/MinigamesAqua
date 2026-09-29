using UnityEngine;
using UnityEngine.UI;

public class RepairBar : MonoBehaviour
{
    [SerializeField] private Image repairBar;
    [SerializeField] private int maxRepairs = 10;

    private int currentRepairs;

    public bool IsComplete => currentRepairs >= maxRepairs;

    private void Start()
    {
        ResetProgress();
    }

    public void AddRepair()
    {
        if (IsComplete)
            return;

        currentRepairs++;

        UpdateBar();
    }

    private void UpdateBar()
    {
        repairBar.fillAmount = (float)currentRepairs / maxRepairs;
    }

    public void ResetProgress()
    {
        currentRepairs = 0;
        UpdateBar();
    }
}