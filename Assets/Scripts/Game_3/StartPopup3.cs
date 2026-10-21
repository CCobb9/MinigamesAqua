
using UnityEngine;
using UnityEngine.InputSystem;

public class StartPopup3 : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject blurOverlay;

    [Header("Game")]
    [SerializeField] private GameManager3 gameManager;

    private bool hasStarted;

    private void Start()
    {
        if (popup != null)
            popup.SetActive(true);

        if (blurOverlay != null)
            blurOverlay.SetActive(true);

        if (gameManager != null)
            gameManager.SetGameStarted(false);
    }

    private void Update()
    {
        if (hasStarted)
            return;

        bool mouseClicked =
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        bool touched =
            Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame;

        if (mouseClicked || touched)
            StartGame();
    }

    private void StartGame()
    {
        hasStarted = true;

        if (popup != null)
            popup.SetActive(false);

        if (blurOverlay != null)
            blurOverlay.SetActive(false);

        if (gameManager != null)
            gameManager.SetGameStarted(true);
    }
}
