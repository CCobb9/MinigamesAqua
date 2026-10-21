using System.Collections.Generic;
using UnityEngine;

public class WaterClickable : MonoBehaviour
{
    private const string WaterBurstSound = "WaterBurst";
    private const string RepairBurstSound = "RepairBurst";

    private static readonly HashSet<WaterClickable> brokenClickables = new();

    [SerializeField] private Animator animator;

    private GameManager gameManager;
    private bool isBroken;

    public bool IsBroken => isBroken;

    public void Initialize(GameManager manager)
    {
        gameManager = manager;
    }

    public void Break()
    {
        if (isBroken)
            return;

        isBroken = true;

        animator.SetBool("IsBroken", true);

        brokenClickables.Add(this);

        if (brokenClickables.Count == 1 &&
            AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLoopSFX(WaterBurstSound);
        }
    }

    public void Repair()
    {
        if (!isBroken)
            return;

        isBroken = false;
        animator.SetBool("IsBroken", false);

        brokenClickables.Remove(this);

        if (brokenClickables.Count == 0 &&
            AudioManager.Instance != null)
        {
            AudioManager.Instance.StopLoopSFX(WaterBurstSound);
        }
    }

    private void OnMouseDown()
    {
        if (!isBroken || gameManager == null)
            return;

        gameManager.TryRepair(this);
        AudioManager.Instance.PlaySFX(RepairBurstSound);
    }

    private void OnDestroy()
    {
        if (!brokenClickables.Remove(this))
            return;

        if (brokenClickables.Count == 0 &&
            AudioManager.Instance != null)
        {
            AudioManager.Instance.StopLoopSFX(WaterBurstSound);
        }
    }
}
