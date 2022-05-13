using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Operation operation;
    [SerializeField] private float tooltipDelay;

    private static readonly Dictionary<Operation, string> explanations = new Dictionary<Operation, string>()
    {
        { Operation.Convert, "" },
        { Operation.Swap, "" },
        { Operation.Promote, "" },
        { Operation.Demote, "" },
        { Operation.Reinforce, "" },
        { Operation.None, "Cancels the current operation. No moves will be deducted from the remaining count." }
    };

    private void OnDisable()
    {
        HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Invoke(nameof(ShowTooltip), tooltipDelay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        TooltipManager.Instance.Show(name, explanations[operation]);
    }

    private void HideTooltip()
    {
        CancelInvoke();
        TooltipManager.Instance.Hide();
    }
}
