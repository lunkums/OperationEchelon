using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;

    public static TooltipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Show(string header, string info)
    {
        tooltip.SetText(header, info);
        tooltip.Active = true;
    }

    public void Hide()
    {
        tooltip.Active = false;
    }
}
