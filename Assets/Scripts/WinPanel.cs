using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private void OnEnable()
    {
        level.OnWin += EnableWinPanel;
        level.OnRestart += DisableWinPanel;
    }

    private void OnDisable()
    {
        level.OnWin -= EnableWinPanel;
        level.OnRestart -= DisableWinPanel;
    }

    private void EnableWinPanel(bool hasWon)
    {
        winPanel.SetActive(hasWon);
        losePanel.SetActive(!hasWon);
    }

    private void DisableWinPanel()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
}
