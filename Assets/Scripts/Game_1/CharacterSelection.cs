using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private GameObject maleCharacter;
    [SerializeField] private GameObject femaleCharacter;

    [Header("UI")]
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject blurOverlay;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    private bool characterSelected;

    private void Start()
    {
        // maleCharacter.SetActive(false);
        // femaleCharacter.SetActive(false);
    }

    public void SelectMale()
    {
        SelectCharacter(maleCharacter);
    }

    public void SelectFemale()
    {
        SelectCharacter(femaleCharacter);
    }

    private void SelectCharacter(GameObject selectedCharacter)
    {
        if (characterSelected)
            return;

        characterSelected = true;

        // maleCharacter.SetActive(selectedCharacter == maleCharacter);
        // femaleCharacter.SetActive(selectedCharacter == femaleCharacter);

        characterPanel.SetActive(false);
        blurOverlay.SetActive(false);

        gameManager.SetGameStarted(true);
    }
}