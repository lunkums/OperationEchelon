using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private ButtonController buttonController;
    [SerializeField] private TMP_Text operationText;
    [SerializeField] private TMP_Text selectedRowText;

    private void OnEnable()
    {
        buttonController.OnSetOperation += SetOperationText;
        buttonController.OnRowSelect += SetSelectedRowText;
    }

    private void OnDisable()
    {
        buttonController.OnSetOperation -= SetOperationText;
        buttonController.OnRowSelect -= SetSelectedRowText;
    }

    public void SetSelectedRowText(int row)
    {
        selectedRowText.text = "Selected row " + (row + 1) + ", select another row";
    }

    public void SetOperationText(Operation operation)
    {
        operationText.text = operation.SelectionText();
        selectedRowText.text = "";
    }
}
