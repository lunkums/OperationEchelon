using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private float showPanelDelay;
    [SerializeField] private GameObject maximizeButton;
    [SerializeField] private GameObject minimizeButton;

    private bool levelOver;
    private bool hasWon;

    private void Awake()
    {
        levelOver = false;
        hasWon = false;
    }

    private void OnEnable()
    {
        level.OnWin += SetWinPanel;
        level.OnRestart += ResetWinPanel;
    }

    private void OnDisable()
    {
        level.OnWin -= SetWinPanel;
        level.OnRestart -= ResetWinPanel;
    }

    public void DisableWinPanel()
    {
        CancelInvoke();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        EnableMinimizeButton(false);
    }

    public void EnableWinPanel()
    {
        winPanel.SetActive(hasWon && levelOver);
        losePanel.SetActive(!hasWon && levelOver);
        EnableMinimizeButton(true);
    }

    public void EnableMinimizeButton(bool active)
    {
        minimizeButton.SetActive(active && levelOver);
        maximizeButton.SetActive(!active && levelOver);
    }

    public void Continue()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    private void SetWinPanel(bool hasWon)
    {
        this.hasWon = hasWon;
        levelOver = true;
        EnableWinPanel(showPanelDelay);
    }

    private void ResetWinPanel()
    {
        levelOver = hasWon = false;
        DisableWinPanel();
    }

    private void EnableWinPanel(float delayInSeconds)
    {
        Invoke(nameof(EnableWinPanel), delayInSeconds);
    }
}
