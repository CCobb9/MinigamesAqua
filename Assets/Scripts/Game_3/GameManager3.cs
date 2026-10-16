
using System.Collections.Generic;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private SequenceButton[] sequenceButtons;
    [SerializeField] private Transform layout;

    [Header("Game Settings")]
    [SerializeField] private int maxErrors = 3;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private int nextExpectedOrder = 1;
    private int currentErrors;
    private bool gameEnded;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        InitializeButtons();
        ShuffleButtons();
    }

    private void InitializeButtons()
    {
        foreach (SequenceButton sequenceButton in sequenceButtons)
        {
            if (sequenceButton == null)
                continue;

            sequenceButton.Initialize(this);
            sequenceButton.ClearIndicators();
            sequenceButton.SetInteractable(true);
        }
    }

    private void ShuffleButtons()
    {
        List<SequenceButton> shuffledButtons =
            new List<SequenceButton>(sequenceButtons);

        for (int i = shuffledButtons.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            (shuffledButtons[i], shuffledButtons[randomIndex]) =
                (shuffledButtons[randomIndex], shuffledButtons[i]);
        }

        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            shuffledButtons[i].transform.SetParent(layout, false);
            shuffledButtons[i].transform.SetSiblingIndex(i);
        }
    }

    public void SelectButton(SequenceButton selectedButton)
    {
        if (gameEnded || selectedButton == null)
            return;

        if (selectedButton.OrderNumber == nextExpectedOrder)
        {
            HandleCorrectAnswer(selectedButton);
        }
        else
        {
            HandleWrongAnswer(selectedButton);
        }
    }

    private void HandleCorrectAnswer(SequenceButton selectedButton)
    {
        selectedButton.ShowCorrect();
        selectedButton.SetInteractable(false);

        foreach (SequenceButton sequenceButton in sequenceButtons)
        {
            if (sequenceButton != null)
                sequenceButton.ClearWrongIndicator();
        }

        nextExpectedOrder++;

        if (nextExpectedOrder > sequenceButtons.Length)
            WinGame();
    }

    private void HandleWrongAnswer(SequenceButton selectedButton)
    {
        selectedButton.ShowWrong();
        currentErrors++;

        if (currentErrors >= maxErrors)
            LoseGame();
    }

    private void WinGame()
    {
        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    private void LoseGame()
    {
        gameEnded = true;

        foreach (SequenceButton sequenceButton in sequenceButtons)
        {
            if (sequenceButton != null)
                sequenceButton.SetInteractable(false);
        }

        if (losePanel != null)
            losePanel.SetActive(true);
    }
}
