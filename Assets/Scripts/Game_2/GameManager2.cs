
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    [Header("Game Systems")]
    [SerializeField] private WaterClickable2[] clickables;
    [SerializeField] private WaterBar2 waterBar;
    [SerializeField] private RepairBar2 repairBar;

    [Header("Leak Settings")]
    [SerializeField, Range(1, 2)] private int maxActiveLeaks = 2;
    [SerializeField] private float timeBetweenBreaks = 3f;

    [Header("Repair")]
    [SerializeField] private RepairAnimationController repairAnimation;

    [Header("UI")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    [SerializeField] private AudioClip backgroundMusic;

    private readonly List<WaterClickable2> activeLeaks = new();

    private WaterClickable2 pendingRepair;
    private Coroutine replenishCoroutine;

    private int targetActiveLeaks;
    private bool isMaleSelected = true;
    private bool gameStarted;
    private bool gameEnded;
    private bool isRepairing;

    private void Start()
    {
        if (losePanel != null)
            losePanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        waterBar.Initialize(this);
        waterBar.ResetProgress();

        foreach (WaterClickable2 clickable in clickables)
        {
            if (clickable == null)
                continue;

            clickable.Initialize(this);
            clickable.Repair();
        }

        if (AudioManager.Instance != null && backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }

    public void SetSelectedCharacter(bool isMale)
    {
        isMaleSelected = isMale;
    }

    public void SetGameStarted(bool started)
    {
        if (!started || gameStarted || gameEnded)
            return;

        gameStarted = true;

        int availableCount = clickables.Length;

        if (availableCount == 0)
            return;

        targetActiveLeaks = Random.Range(
            1,
            Mathf.Min(maxActiveLeaks, availableCount) + 1
        );

        for (int i = 0; i < targetActiveLeaks; i++)
        {
            if (!BreakRandomClickable())
                break;
        }

        waterBar.SetActiveLeaks(activeLeaks.Count);
    }

    private bool BreakRandomClickable()
    {
        List<WaterClickable2> available = new();

        foreach (WaterClickable2 clickable in clickables)
        {
            if (clickable != null &&
                !clickable.IsBroken)
            {
                available.Add(clickable);
            }
        }

        if (available.Count == 0)
            return false;

        WaterClickable2 selected =
            available[Random.Range(0, available.Count)];

        selected.Break();
        activeLeaks.Add(selected);

        return true;
    }

    public void TryRepair(WaterClickable2 clickable)
    {
        if (gameEnded || !gameStarted || isRepairing)
            return;

        if (clickable == null || !activeLeaks.Contains(clickable))
            return;

        isRepairing = true;
        pendingRepair = clickable;

        clickable.ShowRepairVisual(isMaleSelected);

        repairAnimation.PlayAt(clickable.transform.position);
    }

    public void CompleteRepairAnimation()
    {
        if (gameEnded || pendingRepair == null)
            return;

        WaterClickable2 repaired = pendingRepair;

        repairAnimation.Hide();
        repaired.HideRepairVisual();
        repaired.Repair();

        activeLeaks.Remove(repaired);

        pendingRepair = null;
        isRepairing = false;

        repairBar.AddRepair();
        waterBar.SetActiveLeaks(activeLeaks.Count);

        if (repairBar.IsComplete)
        {
            WinGame();
            return;
        }

        ScheduleReplenishment();
    }

    private void ScheduleReplenishment()
    {
        if (replenishCoroutine == null &&
            activeLeaks.Count < targetActiveLeaks)
        {
            replenishCoroutine =
                StartCoroutine(ReplenishLeaksRoutine());
        }
    }

    private IEnumerator ReplenishLeaksRoutine()
    {
        while (!gameEnded &&
               activeLeaks.Count < targetActiveLeaks)
        {
            yield return new WaitForSeconds(timeBetweenBreaks);

            if (gameEnded)
                break;

            if (!BreakRandomClickable())
                break;

            waterBar.SetActiveLeaks(activeLeaks.Count);
        }

        replenishCoroutine = null;
    }

    public void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        StopReplenishment();

        waterBar.SetActiveLeaks(0);

        if (pendingRepair != null)
            pendingRepair.HideRepairVisual();

        if (repairAnimation != null)
            repairAnimation.Hide();

        pendingRepair = null;
        isRepairing = false;

        if (losePanel != null)
            losePanel.SetActive(true);
        AudioManager.Instance.PlaySFX("LosePanel");
    }

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        StopReplenishment();

        waterBar.SetActiveLeaks(0);

        if (pendingRepair != null)
            pendingRepair.HideRepairVisual();

        if (repairAnimation != null)
            repairAnimation.Hide();

        pendingRepair = null;
        isRepairing = false;

        if (winPanel != null)
            winPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("WinPanel");
    }

    private void StopReplenishment()
    {
        if (replenishCoroutine == null)
            return;

        StopCoroutine(replenishCoroutine);
        replenishCoroutine = null;
    }
}
