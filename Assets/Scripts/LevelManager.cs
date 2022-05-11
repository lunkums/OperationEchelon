using System;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level currentLevel;
    [SerializeField] private Sound winSfx;
    [SerializeField] private Sound loseSfx;
    [SerializeField] private TextAsset[] levelFiles;

    private int indexOfCurrentLevel;

    public static LevelManager Instance { get; private set; }

    public Level CurrentLevel => currentLevel;
    public int IndexOfCurrentLevel => indexOfCurrentLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
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
        Debug.Log("Loading level : " + levelFiles[indexOfCurrentLevel = level].name);
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
        GUI.Label(new Rect(0, 0, 100, 100), "Level : " + (indexOfCurrentLevel + 1) + "/" + levelFiles.Length);
    }
}
