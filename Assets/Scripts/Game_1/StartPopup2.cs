using UnityEngine;
using UnityEngine.InputSystem;

public class StartPopup2 : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private GameObject characterPanel;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    private bool hasOpenedCharacterSelection;

    private void Start()
    {
        popup.SetActive(true);
        blurOverlay.SetActive(true);
        characterPanel.SetActive(false);

        gameManager.SetGameStarted(false);
    }

    private void Update()
    {
        if (hasOpenedCharacterSelection)
            return;

        bool mouseClicked =
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        bool touched =
            Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame;

        if (mouseClicked || touched)
            OpenCharacterSelection();
    }

    private void OpenCharacterSelection()
    {
        hasOpenedCharacterSelection = true;

        popup.SetActive(false);
        blurOverlay.SetActive(false);
        characterPanel.SetActive(true);
    }
}