using System.Collections;
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

    public void Continue()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    private void EnableWinPanel(bool hasWon)
    {
        StartCoroutine(DelayedEnableWinPanel(hasWon));
    }

    private void DisableWinPanel()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private IEnumerator DelayedEnableWinPanel(bool hasWon)
    {
        yield return new WaitForSeconds(1);
        winPanel.SetActive(hasWon);
        losePanel.SetActive(!hasWon);
    }
}
