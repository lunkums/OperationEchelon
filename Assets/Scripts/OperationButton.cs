using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Operation operation;

    private static readonly Dictionary<Operation, string> explanations = new Dictionary<Operation, string>()
    {
        { Operation.Convert, "Converts every Troop in a row to the opposite color, regardless of Rank. Empty spaces remain unchanged." },
        { Operation.Swap, "Swaps two rows with each other." },
        { Operation.Promote, "Increases the Rank of every Troop in a row by one, regardless of color. Privates become Sergeants, Sergeants become Captains, and so on. Cannot Promote a row containing a General. Empty spaces remain unchanged." },
        { Operation.Demote, "Decreases the Rank of every Troop in a row by one, regardless of color. Generals become Captains, Captains become Sergeants, and so on. Cannot Demote a row containing a Private. Empty spaces remain unchanged." },
        { Operation.Reinforce, "Adds the first selected row to the second one. Reinforcing a Troop with another of the same color Promotes it. If their colors aren't the same, the Troop being reinforced may change color and Rank or become empty. Empty spaces become whatever the Reinforcing Troop is. Cannot Reinforce if it results in a Rank higher than General. The first selected row remains unchanged." },
        { Operation.None, "Cancels the current operation. No moves are deducted from the remaining count." }
    };

    private void OnDisable()
    {
        TooltipManager.Instance.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.Show(name, explanations[operation]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}
