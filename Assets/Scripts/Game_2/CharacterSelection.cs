
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject blurOverlay;

    [Header("Game")]
    [SerializeField] private GameManager2 gameManager;

    private bool characterSelected;

    public void SelectMale()
    {
        SelectCharacter(true);
    }

    public void SelectFemale()
    {
        SelectCharacter(false);
    }

    private void SelectCharacter(bool isMale)
    {
        if (characterSelected || gameManager == null)
            return;

        characterSelected = true;

        gameManager.SetSelectedCharacter(isMale);

        characterPanel.SetActive(false);
        blurOverlay.SetActive(false);

        gameManager.SetGameStarted(true);
    }
}
