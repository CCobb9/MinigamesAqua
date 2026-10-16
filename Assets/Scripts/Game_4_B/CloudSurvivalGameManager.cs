
using System.Collections;
using TMPro;
using UnityEngine;

public class CloudSurvivalGameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private CloudPlayerController cloud;
    [SerializeField] private RectTransform cloudRect;

    [Header("Airplanes")]
    [SerializeField] private AirplaneSpawner airplaneSpawner;

    [Header("Collision Settings")]
    [Range(0f, 0.45f)]
    [SerializeField] private float cloudCollisionReduction = 0.15f;

    [Range(0f, 0.45f)]
    [SerializeField] private float airplaneCollisionReduction = 0.25f;

    [Header("Timer")]
    [SerializeField] private float timeLimit = 30f;
    [SerializeField] private TMP_Text timerText;

    [Header("Win Animation")]
    [SerializeField] private Animator cloudAnimator;
    [SerializeField] private string winAnimationTrigger = "Win";
    [SerializeField] private float moveToCenterDuration = 1f;
    [SerializeField] private float winPopupDelay = 3f;

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private float timeRemaining;
    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        if (cloud != null)
            cloud.SetCanMove(false);

        timeRemaining = timeLimit;
        UpdateTimerText();
    }

    private void Update()
    {
        if (!gameStarted || gameEnded)
            return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0f, timeRemaining);

        UpdateTimerText();

        if (CheckCollision())
        {
            LoseGame();
            return;
        }

        if (timeRemaining <= 0f)
            WinGame();
    }

    public void SetGameStarted(bool started)
    {
        if (!started || gameStarted || gameEnded)
            return;

        gameStarted = true;
        timeRemaining = timeLimit;

        if (cloud != null)
            cloud.SetCanMove(true);

        if (airplaneSpawner != null)
            airplaneSpawner.StartSpawning();

        UpdateTimerText();
    }

    private bool CheckCollision()
    {
        if (cloudRect == null || airplaneSpawner == null)
            return false;

        Rect cloudBounds = GetScreenRect(
            cloudRect,
            cloudCollisionReduction
        );

        foreach (Airplane airplane in airplaneSpawner.ActiveAirplanes)
        {
            if (airplane == null || !airplane.gameObject.activeInHierarchy)
                continue;

            RectTransform airplaneRect =
                airplane.GetComponent<RectTransform>();

            if (airplaneRect == null)
                continue;

            Rect airplaneBounds = GetScreenRect(
                airplaneRect,
                airplaneCollisionReduction
            );

            if (cloudBounds.Overlaps(airplaneBounds))
                return true;
        }

        return false;
    }

    private Rect GetScreenRect(
        RectTransform rectTransform,
        float reduction)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();

        Camera uiCamera = null;

        if (canvas != null &&
            canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }

        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(
            uiCamera, corners[0]);

        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(
            uiCamera, corners[2]);

        Rect rect = Rect.MinMaxRect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x,
            topRight.y
        );

        float widthReduction = rect.width * reduction;
        float heightReduction = rect.height * reduction;

        rect.xMin += widthReduction;
        rect.xMax -= widthReduction;
        rect.yMin += heightReduction;
        rect.yMax -= heightReduction;

        return rect;
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int totalSeconds = Mathf.CeilToInt(timeRemaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        gameStarted = false;

        if (cloud != null)
            cloud.SetCanMove(false);

        if (airplaneSpawner != null)
            airplaneSpawner.ClearAirplanes();

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        gameStarted = false;

        if (airplaneSpawner != null)
            airplaneSpawner.ClearAirplanes();

        StartCoroutine(ShowWinSequence());
    }

    private IEnumerator ShowWinSequence()
    {
        if (cloud != null)
            cloud.MoveToCenter(moveToCenterDuration);

        yield return new WaitForSeconds(moveToCenterDuration);

        if (cloudAnimator != null)
            cloudAnimator.enabled = true;

        yield return new WaitForSeconds(winPopupDelay);

        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
