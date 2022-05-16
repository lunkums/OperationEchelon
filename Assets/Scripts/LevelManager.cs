using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private StatsLog statsLog;
    [SerializeField] private Level currentLevel;
    [SerializeField] private Sound winSfx;
    [SerializeField] private Sound loseSfx;
    [SerializeField] private TextAsset[] levelFiles;

    private int indexOfCurrentLevel;

    public static LevelManager Instance { get; private set; }

    public Level CurrentLevel => currentLevel;
    public int IndexOfCurrentLevel => indexOfCurrentLevel;
    public string NameOfCurrentLevel => levelFiles[IndexOfCurrentLevel].name;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        if (Application.platform != RuntimePlatform.WebGLPlayer)
            statsLog.enabled = true;
    }

    private void Start()
    {
        currentLevel.CreateLevelFromText(levelFiles[indexOfCurrentLevel = 0]);
    }

    private void OnEnable()
    {
        currentLevel.OnWin += PlayWinSoundEffect;
    }

    private void OnDisable()
    {
        currentLevel.OnWin -= PlayWinSoundEffect;
    }

    public void ReturnToMenu()
    {

    }

    public void LoadNextLevel()
    {
        LoadLevelFromIndex((indexOfCurrentLevel + 1) % levelFiles.Length);
    }

    public void LoadPreviousLevel()
    {
        LoadLevelFromIndex(Math.Max(indexOfCurrentLevel - 1, 0));
    }

    public void LoadLevelFromIndex(int level)
    {
        currentLevel.CreateLevelFromText(levelFiles[indexOfCurrentLevel = level]);
    }

    private void PlayWinSoundEffect(bool hasWon)
    {
        string sfxName;

        if (hasWon)
            sfxName = winSfx.Name;
        else
            sfxName = loseSfx.Name;

        AudioManager.Instance.Play(sfxName);
    }

    private void OnGUI()
    {
        float width = 175;
        float height = 100;
        GUI.Label(new Rect(Screen.width - width, Screen.height - height, width, height), "Level : " + (indexOfCurrentLevel + 1) + "/" + levelFiles.Length + " [" + levelFiles[indexOfCurrentLevel].name + "]\nPress 'n' to go to the next level\nPress 'b' to go back\nPress 'r' to restart the level\nPress 'q' to quit");
    }
}
