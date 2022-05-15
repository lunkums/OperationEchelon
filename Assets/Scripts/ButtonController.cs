using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Sound buttonSound;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject[] operationButtons;
    [SerializeField] private Level level;
    [SerializeField] private Formation currentFormation;
    [SerializeField] private GameObject rowSelector;
    [SerializeField] private float secondsBeforeShowPreview;
    [SerializeField] private float mouseMoveLeeway;
    [SerializeField] private Formation preview;

    private List<GameObject> activeOperationButtons;

    // Update behaviour (mouse listening state or not)
    private Action updateBehaviour;
    // Variables for selecting rows that are used in Operations
    private int[] selections;
    private int selectionsNeeded;
    private int selectionsMade;

    private Vector3 lastMousePos;
    private float timer;
    private Vector3 currentFormationPos;

    public bool CanPerformOperations { get; private set; }
    public Operation CurrentOperation { get; private set; }

    public event Action<Operation> OnSetOperation;
    public event Action<int> OnRowSelect;

    private void Awake()
    {
        activeOperationButtons = new List<GameObject>();
        lastMousePos = Input.mousePosition;
        timer = secondsBeforeShowPreview;
        currentFormationPos = currentFormation.Position;
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
        updateBehaviour = ListenForRowSelection;
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
        rowSelector.SetActive(false);
        updateBehaviour = () => { };
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.Play(buttonSound.Name);
    }

    private void Restart()
    {
        ClearPreview();
        updateBehaviour = () => { };
        selectionsNeeded = 0;
        CanPerformOperations = true;
        cancelButton.SetActive(false);
        rowSelector.SetActive(false);
        ActivateOperationButtons(false);
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
    private void ListenForRowSelection()
    {
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseClicked = Input.GetMouseButtonDown(0);
        bool hasMouseMoved = Vector3.Distance(lastMousePos, currentMousePos) > mouseMoveLeeway;
        bool validRowSelection = currentFormation.TrySelectRow(new Vector3(currentMousePos.x, currentMousePos.y, currentFormation.Position.z), out int row);
        lastMousePos = currentMousePos;
        timer -= Time.deltaTime;

        if (hasMouseMoved || mouseClicked || !validRowSelection)
            ClearPreview();

        if (validRowSelection)
        {
            selections[selectionsMade] = row;
            if (mouseClicked)
                SelectRow();
            else if (!hasMouseMoved && timer < 0)
                PreviewMove();
        }
    }

    private void SelectRow()
    {
        rowSelector.SetActive(true);
        rowSelector.transform.position = currentFormation.RowSelectorPosition + 3 * Vector3.back;

        if (selections[0] != selections[1])
            selectionsMade++;
        OnRowSelect.Invoke(selections[selectionsMade - 1]);

        if (selectionsMade == selectionsNeeded)
        {
            currentFormation.ApplyMove(new Move(CurrentOperation, selections));
            ClearOperation();
        }
    }

    private void PreviewMove()
    {
        if (selectionsMade != selectionsNeeded - 1)
            return;

        preview.Position = Vector3.back;
        currentFormation.Position = Vector3.forward;
        preview.Active = true;
        preview.Copy(currentFormation);
        preview.ApplyMove(new Move(CurrentOperation, selections));
    }

    private void ClearPreview()
    {
        currentFormation.Position = currentFormationPos;
        preview.Position = Vector3.forward;
        preview.Clear();
        preview.Active = false;
        timer = secondsBeforeShowPreview;
    }
}
