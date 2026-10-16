
using UnityEngine;
using UnityEngine.InputSystem;

public class StartPopup4D : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject blurOverlay;

    [Header("Game Manager")]
    [SerializeField] private MountainWaterGameManager gameManager;

    private bool hasStarted;

    private void Start()
    {
        popup.SetActive(true);
        blurOverlay.SetActive(true);

        if (gameManager != null)
            gameManager.SetGameStarted(false);
    }

    private void Update()
    {
        if (hasStarted)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        hasStarted = true;

        popup.SetActive(false);
        blurOverlay.SetActive(false);

        if (gameManager != null)
            gameManager.SetGameStarted(true);
    }
}
