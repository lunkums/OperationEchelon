using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private TMP_Text operationText;
    [SerializeField] private TMP_Text selectedRowText;
    [SerializeField] private Level level;

    private Dictionary<bool, string> levelOverMessage;

    private void Awake()
    {
        levelOverMessage = new Dictionary<bool, string>()
        {
            {true, "Success! You have achieved the optimal formation." },
            {false, "Failure! You have not achieved the optimal formation." }
        };
    }

    private void OnEnable()
    {
        buttonController.OnSetOperation += LogOperation;
        buttonController.OnRowSelect += LogRowSelection;
        level.OnWin += LogWinStatus;
        level.OnRestart += () => SetOperationText(Operation.None.SelectionText());
    }

    private void OnDisable()
    {
        buttonController.OnSetOperation -= LogOperation;
        buttonController.OnRowSelect -= LogRowSelection;
        level.OnWin -= LogWinStatus;
        level.OnRestart -= () => SetOperationText(Operation.None.SelectionText());
    }

    private void LogRowSelection(int row)
    {
        selectedRowText.text = "Selected row " + (row + 1) + ", select another row";
    }

    private void LogOperation(Operation operation)
    {
        if (!buttonController.CanPerformOperations)
            return;

        SetOperationText(operation.SelectionText());
    }

    private void LogWinStatus(bool hasWon)
    {
        SetOperationText(levelOverMessage[hasWon]);
    }

    private void SetOperationText(string text)
    {
        operationText.text = text;
        selectedRowText.text = "";
    }
}
