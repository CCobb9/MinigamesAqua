
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager3 : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private SequenceButton[] sequenceButtons;
    [SerializeField] private Transform layout;

    [Header("Game Settings")]
    [SerializeField] private int maxErrors = 2;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Attempts UI")]
    [SerializeField] private TMP_Text attemptsText;

    [SerializeField] private AudioClip backgroundMusic;

    private int nextExpectedOrder = 1;
    private int currentErrors;
    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        InitializeButtons();
        ShuffleButtons();

        currentErrors = 0;
        UpdateAttemptsText();

        if (AudioManager.Instance != null && backgroundMusic != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }

    public void SetGameStarted(bool started)
    {
        gameStarted = started;
    }

    private void UpdateAttemptsText()
    {
        if (attemptsText == null)
            return;

        int remainingAttempts = Mathf.Max(0, maxErrors - currentErrors);

        attemptsText.text = remainingAttempts.ToString();
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
        if (!gameStarted || gameEnded || selectedButton == null)
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

        UpdateAttemptsText();

        if (currentErrors >= maxErrors)
            LoseGame();
    }

    private void WinGame()
    {
        gameEnded = true;

        if (winPanel != null)
            winPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("WinPanel");
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
        AudioManager.Instance.PlaySFX("LosePanel");
    }
}
