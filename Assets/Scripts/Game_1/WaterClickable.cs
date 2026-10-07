using UnityEngine;

public class WaterClickable : MonoBehaviour
{
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
        isBroken = true;

        animator.SetBool("IsBroken", true);
    }

    public void Repair()
    {
        isBroken = false;

        animator.SetBool("IsBroken", false);
    }

    private void OnMouseDown()
    {
        if (!isBroken)
            return;

        gameManager.TryRepair(this);
    }
}