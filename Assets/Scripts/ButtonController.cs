using System;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Sound buttonSound;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject[] operationButtons;

    // Update behaviour (mouse listening state or not)
    private Action updateBehaviour;
    // Variables for selecting rows that are used in operations
    private int[] selections;
    private int selectionsNeeded;
    private int selectionsMade;

    private void Start()
    {
        updateBehaviour = () => { };
        selectionsNeeded = 0;
    }

    public Operation CurrentOperation { get; private set; }

    public event Action<Operation> OnSetOperation;
    public event Action<int> OnRowSelect;

    public void SetOperation(int operation)
    {
        CurrentOperation = (Operation)operation;
        updateBehaviour = GetRow;
        // Get the selection variables ready
        selectionsNeeded = CurrentOperation.SelectionsNeeded();
        selectionsMade = 0;
        selections = new int[] { -1, -1 };
        // Update the buttons
        foreach (GameObject button in operationButtons)
            button.SetActive(false);
        cancelButton.SetActive(true);
        OnSetOperation.Invoke(CurrentOperation);
    }

    public void ClearOperation()
    {
        SetOperation((int)Operation.None);
        foreach (GameObject button in operationButtons)
            button.SetActive(true);
        cancelButton.SetActive(false);
        updateBehaviour = () => { };
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.Play(buttonSound.Name);
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

        if (hit.collider != null && hit.transform.TryGetComponent(out formation))
        {
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

    private void Update()
    {
        updateBehaviour.Invoke();
    }
}
