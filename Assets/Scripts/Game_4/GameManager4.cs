
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager4 : MonoBehaviour
{
    [Header("Clouds")]
    [SerializeField] private CloudTarget cloudPrefab;
    [SerializeField] private RectTransform cloudContainer;
    [SerializeField] private int cloudCount = 40;

    [Header("Timer")]
    [SerializeField] private float timeLimit = 30f;
    [SerializeField] private TMP_Text timerText;

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Win Animation")]
    [SerializeField] private GameObject sunAnimation;
    [SerializeField] private GameObject vaporAnimation;
    [SerializeField] private float winPopupDelay = 3f;

    private int remainingClouds;
    private float timeRemaining;
    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (sunAnimation != null) sunAnimation.SetActive(false);
        if (vaporAnimation != null) vaporAnimation.SetActive(false);
    }

    private void Update()
    {

        if (!gameStarted || gameEnded)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();

            LoseGame();
            return;
        }

        UpdateTimerText();
    }


    private void SpawnClouds()
    {
        RectTransform containerRect = cloudContainer;

        for (int i = 0; i < cloudCount; i++)
        {
            CloudTarget cloud = Instantiate(cloudPrefab, containerRect);
            cloud.Initialize(this);

            RectTransform cloudRect = cloud.GetComponent<RectTransform>();

            float halfWidth = containerRect.rect.width / 2f;
            float halfHeight = containerRect.rect.height / 2f;

            float cloudHalfWidth = cloudRect.rect.width * cloudRect.localScale.x / 2f;
            float cloudHalfHeight = cloudRect.rect.height * cloudRect.localScale.y / 2f;

            float randomX = Random.Range(
                -halfWidth + cloudHalfWidth,
                halfWidth - cloudHalfWidth
            );

            float randomY = Random.Range(
                -halfHeight + cloudHalfHeight,
                halfHeight - cloudHalfHeight
            );

            cloudRect.anchoredPosition = new Vector2(randomX, randomY);
        }
    }


    public void RemoveCloud(CloudTarget cloud)
    {
        if (gameEnded || cloud == null)
            return;

        cloud.Remove();
        remainingClouds--;

        if (remainingClouds <= 0)
            WinGame();
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int totalSeconds = Mathf.CeilToInt(timeRemaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void SetGameStarted(bool started)
    {
        gameStarted = started;

        if (!started)
            return;

        timeRemaining = timeLimit;
        remainingClouds = cloudCount;
        gameEnded = false;

        SpawnClouds();
        UpdateTimerText();
    }

    private void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        StartCoroutine(ShowWinPopup());
    }

    private IEnumerator ShowWinPopup()
    {
        if (sunAnimation != null)
            sunAnimation.SetActive(true);

        if (vaporAnimation != null)
            vaporAnimation.SetActive(true);

        yield return new WaitForSeconds(winPopupDelay);

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    private void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        if (losePanel != null)
            losePanel.SetActive(true);
    }
}
