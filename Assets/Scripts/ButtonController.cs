using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Sound buttonSound;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject[] operationButtons;
    [SerializeField] private Level level;

    private List<GameObject> activeOperationButtons;

    // Update behaviour (mouse listening state or not)
    private Action updateBehaviour;
    // Variables for selecting rows that are used in Operations
    private int[] selections;
    private int selectionsNeeded;
    private int selectionsMade;

    public bool CanPerformOperations { get; private set; }
    public Operation CurrentOperation { get; private set; }

    public event Action<Operation> OnSetOperation;
    public event Action<int> OnRowSelect;

    private void Awake()
    {
        activeOperationButtons = new List<GameObject>();
    }

    private void OnEnable()
    {
        level.OnRestart += Restart;
        level.OnWin += (hasWon) => CanPerformOperations = false;
    }

    private void OnDisable()
    {
        level.OnRestart -= Restart;
        level.OnWin -= (hasWon) => CanPerformOperations = false;
    }

    private void Update()
    {
        updateBehaviour.Invoke();
    }

    // Sets the Operations, disables the Operation buttons, and starts polling for a selected row.
    public void SetOperation(int operation)
    {
        CurrentOperation = (Operation)operation;
        updateBehaviour = GetRow;
        // Get the selection variables ready
        selectionsNeeded = CurrentOperation.SelectionsNeeded();
        selectionsMade = 0;
        selections = new int[] { -1, -1 };
        // Update the buttons
        ActivateOperationButtons(false);
        cancelButton.SetActive(true);
        OnSetOperation.Invoke(CurrentOperation);
    }

    // Sets the current Operation to none and ensures this class stops checking for a selected row.
    public void ClearOperation()
    {
        SetOperation((int)Operation.None);
        ActivateOperationButtons(true);
        cancelButton.SetActive(false);
        updateBehaviour = () => { };
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.Play(buttonSound.Name);
    }

    private void AllowOperation(int index)
    {
        activeOperationButtons.Add(operationButtons[index]);
    }

    private void Restart()
    {
        updateBehaviour = () => { };
        selectionsNeeded = 0;
        CanPerformOperations = true;
        activeOperationButtons.Clear();
        foreach (int i in level.AllowableOperations)
            activeOperationButtons.Add(operationButtons[i]);
        ActivateOperationButtons(true);
    }

    private void ActivateOperationButtons(bool active)
    {
        foreach (GameObject button in activeOperationButtons)
            button.SetActive(active && CanPerformOperations);
    }

    /* An update behaviour that checks to see if there is a mouse click within the
       formation and, if so, checks which row was selected. Prompts multiple times for
       operations that require multiple rows. */
    private void GetRow()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Formation formation;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null || !hit.transform.TryGetComponent(out formation))
            return;

        selections[selectionsMade] = formation.SelectRow(mousePos);

        if (selections[0] != selections[1])
            selectionsMade++;
        OnRowSelect.Invoke(selections[selectionsMade - 1]);

        if (selectionsMade != selectionsNeeded)
            return;

        formation.ApplyMove(new Move(CurrentOperation, selections));
        ClearOperation();
    }
}
