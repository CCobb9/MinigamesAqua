using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Systems")]
    [SerializeField] private WaterClickable[] clickables;
    [SerializeField] private WaterBar waterBar;
    [SerializeField] private RepairBar repairBar;

    [Header("Game Settings")]
    [SerializeField] private float timeBetweenBreaks = 3f;

    [Header("UI")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    private WaterClickable currentBrokenClickable;
    private Coroutine nextBreakCoroutine;

    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);

        waterBar.Initialize(this);
        InitializeClickables();
    }

    private void InitializeClickables()
    {
        foreach (WaterClickable clickable in clickables)
        {
            clickable.Initialize(this);
        }
    }

    private void BreakRandomClickable()
    {
        if (!gameStarted || gameEnded)
            return;

        if (repairBar.IsComplete)
            return;

        if (clickables == null || clickables.Length == 0)
            return;

        WaterClickable newClickable;

        do
        {
            newClickable = clickables[Random.Range(0, clickables.Length)];
        }
        while (newClickable == currentBrokenClickable && clickables.Length > 1);

        currentBrokenClickable = newClickable;

        currentBrokenClickable.Break();

        waterBar.StartProgress();
    }

    public void TryRepair(WaterClickable clickable)
    {
        if (gameEnded)
            return;

        if (clickable != currentBrokenClickable)
            return;

        waterBar.StopProgress();

        currentBrokenClickable.Repair();
        currentBrokenClickable = null;

        repairBar.AddRepair();

        if (repairBar.IsComplete)
        {
            WinGame();
            return;
        }

        if (nextBreakCoroutine != null)
            StopCoroutine(nextBreakCoroutine);

        nextBreakCoroutine = StartCoroutine(BreakNextClickable());
    }

    private IEnumerator BreakNextClickable()
    {
        yield return new WaitForSeconds(timeBetweenBreaks);

        BreakRandomClickable();

        nextBreakCoroutine = null;
    }

    public void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (nextBreakCoroutine != null)
        {
            StopCoroutine(nextBreakCoroutine);
            nextBreakCoroutine = null;
        }

        waterBar.StopProgress();

        if (currentBrokenClickable != null)
        {
            currentBrokenClickable.Repair();
            currentBrokenClickable = null;
        }

        losePanel.SetActive(true);
    }

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (nextBreakCoroutine != null)
        {
            StopCoroutine(nextBreakCoroutine);
            nextBreakCoroutine = null;
        }

        waterBar.StopProgress();

        winPanel.SetActive(true);
    }

    public void SetGameStarted(bool started)
    {
        gameStarted = started;

        if (!gameStarted)
            return;

        BreakRandomClickable();
    }
}