using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private float tooltipDelay;

    private string header;
    private string info;

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
        this.header = header;
        this.info = info;
        Invoke(nameof(ActivateTooltip), tooltipDelay);
    }

    public void Hide()
    {
        CancelInvoke();
        tooltip.FadeOut();
    }

    private void ActivateTooltip()
    {
        tooltip.SetText(header, info);
        tooltip.FadeIn();
    }
}
