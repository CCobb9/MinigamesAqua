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

    [Header("Scene Manager")]
    [SerializeField] private GameSceneManager gameSceneManager;
    [SerializeField] private float menuDelay = 4f;

    [SerializeField] private AudioClip backgroundMusic;

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

        if (AudioManager.Instance != null && backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
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

        Image path = index < waterPathFills.Length
            ? waterPathFills[index]
            : null;

        if (path != null)
        {
            float elapsed = 0f;
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX("WaterFlow");

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

        if (rocks[nextRockIndex] != null)
            rocks[nextRockIndex].SetInteractable(true);

        isAnimating = false;
    }

    private void CompleteGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        isAnimating = false;

        if (blockedWaterObject != null)
            blockedWaterObject.SetActive(false);

        if (flowingWaterObject != null)
            flowingWaterObject.SetActive(true);

        if (winPopup != null)
            winPopup.SetActive(true);
        AudioManager.Instance.PlaySFX("WinPanel");

        StartCoroutine(ReturnToMenu());
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(menuDelay);

        if (gameSceneManager != null)
        {
            gameSceneManager.LoadMainTitle();
        }
    }
}