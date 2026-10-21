
using System.Collections.Generic;
using UnityEngine;

public class WaterClickable2 : MonoBehaviour
{
    private const string WaterBurstSound = "WaterBurst";
    private const string RepairBurstSound = "RepairBurst";

    private static readonly HashSet<WaterClickable2> brokenClickables = new();

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Repair Characters")]
    [SerializeField] private GameObject boyRepairVisual;
    [SerializeField] private GameObject girlRepairVisual;

    private GameManager2 gameManager;
    private bool isBroken;

    public bool IsBroken => isBroken;

    public void Initialize(GameManager2 manager)
    {
        gameManager = manager;
        HideRepairVisual();
    }

    private void Awake()
    {
        HideRepairVisual();
    }

    public void Break()
    {
        if (isBroken)
            return;

        isBroken = true;

        if (animator != null)
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

        if (animator != null)
            animator.SetBool("IsBroken", false);

        HideRepairVisual();
        brokenClickables.Remove(this);

        if (brokenClickables.Count == 0 &&
            AudioManager.Instance != null)
        {
            AudioManager.Instance.StopLoopSFX(WaterBurstSound);
        }
    }

    public void ShowRepairVisual(bool isMale)
    {
        if (boyRepairVisual != null)
            boyRepairVisual.SetActive(isMale);

        if (girlRepairVisual != null)
            girlRepairVisual.SetActive(!isMale);
    }

    public void HideRepairVisual()
    {
        if (boyRepairVisual != null)
            boyRepairVisual.SetActive(false);

        if (girlRepairVisual != null)
            girlRepairVisual.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!isBroken || gameManager == null)
            return;

        gameManager.TryRepair(this);

        if (AudioManager.Instance != null)
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
