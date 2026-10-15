
using UnityEngine;

public class WaterClickable2 : MonoBehaviour
{
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
    }

    public void Repair()
    {
        isBroken = false;

        if (animator != null)
            animator.SetBool("IsBroken", false);

        HideRepairVisual();
    }

    public void ShowRepairVisual(bool isMale)
    {
        boyRepairVisual.SetActive(isMale);
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
    }
}
