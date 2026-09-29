using UnityEngine;
using UnityEngine.InputSystem;

public class StartPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private GameManager gameManager;

    private bool hasStarted;

    private void Start()
    {
        popup.SetActive(true);
        blurOverlay.SetActive(true);

        gameManager.SetGameStarted(false);
    }

    private void Update()
    {
        if (hasStarted)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        hasStarted = true;

        popup.SetActive(false);
        blurOverlay.SetActive(false);

        gameManager.SetGameStarted(true);
    }
}