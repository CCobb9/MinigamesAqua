
using UnityEngine;

public class CloudFlappyGameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private CloudFlappyController cloud;

    [Header("Popups")]
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    [SerializeField] private AudioClip backgroundMusic;

    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        if (winPopup != null)
            winPopup.SetActive(false);

        if (losePopup != null)
            losePopup.SetActive(false);

        if (cloud != null)
            cloud.SetCanMove(false);

        if (AudioManager.Instance != null && backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }

    public void SetGameStarted(bool started)
    {
        if (!started || gameStarted || gameEnded)
            return;

        gameStarted = true;

        if (cloud != null)
            cloud.SetCanMove(true);
    }

    public void HandleCollision(Collider2D other)
    {
        if (!gameStarted || gameEnded || other == null)
            return;

        if (other.GetComponent<MountainObstacle>() != null ||
            other.GetComponentInParent<MountainObstacle>() != null)
        {
            LoseGame();
            return;
        }

        if (other.GetComponent<GoalTrigger>() != null ||
            other.GetComponentInParent<GoalTrigger>() != null)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        gameStarted = false;

        if (cloud != null)
            cloud.SetCanMove(false);

        if (winPopup != null)
            winPopup.SetActive(true);

        AudioManager.Instance.PlaySFX("WinPanel");
    }

    private void LoseGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        gameStarted = false;

        if (cloud != null)
            cloud.SetCanMove(false);

        if (losePopup != null)
            losePopup.SetActive(true);
        AudioManager.Instance.PlaySFX("LosePanel");
    }
}
