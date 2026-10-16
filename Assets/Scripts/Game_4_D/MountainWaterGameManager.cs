
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MountainWaterGameManager : MonoBehaviour
{
    [Header("Rocks - Order 1 to 9")]
    [SerializeField] private MountainRockButton[] rocks;

    [Header("Water Paths - Order 1 to 9")]
    [SerializeField] private Image[] waterPathFills;
    [SerializeField] private float fillDuration = 0.5f;

    [Header("Water Flow")]
    [SerializeField] private GameObject blockedWaterObject;
    [SerializeField] private GameObject flowingWaterObject;

    [Header("Win")]
    [SerializeField] private GameObject winPopup;

    private int nextRockIndex;
    private bool isAnimating;
    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        if (winPopup != null)
            winPopup.SetActive(false);

        if (blockedWaterObject != null)
            blockedWaterObject.SetActive(true);

        if (flowingWaterObject != null)
            flowingWaterObject.SetActive(false);

        for (int i = 0; i < waterPathFills.Length; i++)
        {
            if (waterPathFills[i] != null)
                waterPathFills[i].fillAmount = 0f;
        }

        for (int i = 0; i < rocks.Length; i++)
        {
            if (rocks[i] == null)
                continue;

            rocks[i].Initialize(this);
            rocks[i].SetInteractable(i == 0);
        }
    }

    public void SetGameStarted(bool started)
    {
        gameStarted = started;
    }

    public void TryRemoveRock(MountainRockButton rock)
    {
        if (!gameStarted || gameEnded || isAnimating || rock == null)
            return;

        if (nextRockIndex >= rocks.Length)
            return;

        if (rocks[nextRockIndex] != rock)
            return;

        if (rock.OrderNumber != nextRockIndex + 1)
            return;

        rock.Remove();
        StartCoroutine(FillWaterPath(nextRockIndex));
    }

    private IEnumerator FillWaterPath(int index)
    {
        isAnimating = true;

        Image path = waterPathFills[index];

        if (path != null)
        {
            float elapsed = 0f;

            while (elapsed < fillDuration)
            {
                elapsed += Time.deltaTime;

                path.fillAmount = Mathf.Lerp(
                    0f,
                    1f,
                    Mathf.Clamp01(elapsed / fillDuration)
                );

                yield return null;
            }

            path.fillAmount = 1f;
        }

        nextRockIndex++;

        if (nextRockIndex >= rocks.Length)
        {
            CompleteGame();
            yield break;
        }

        rocks[nextRockIndex].SetInteractable(true);

        isAnimating = false;
    }

    private void CompleteGame()
    {
        gameEnded = true;
        isAnimating = false;

        if (blockedWaterObject != null)
            blockedWaterObject.SetActive(false);

        if (flowingWaterObject != null)
            flowingWaterObject.SetActive(true);

        if (winPopup != null)
            winPopup.SetActive(true);
    }
}
