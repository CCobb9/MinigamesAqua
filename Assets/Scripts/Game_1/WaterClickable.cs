using UnityEngine;

public class WaterClickable : MonoBehaviour
{
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
    }

    public void Repair()
    {
        isBroken = false;
    }

    private void OnMouseDown()
    {
        if (!isBroken)
            return;

        gameManager.TryRepair(this);
    }
}