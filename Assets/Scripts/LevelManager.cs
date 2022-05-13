using System;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const string outputLogPath = "output.txt";

    [SerializeField] private Level currentLevel;
    [SerializeField] private Sound winSfx;
    [SerializeField] private Sound loseSfx;
    [SerializeField] private TextAsset[] levelFiles;

    private int indexOfCurrentLevel;
    private StreamWriter outputLog;
    private float timer;
    private int numOfTries;

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

        bool justCreated = !File.Exists(outputLogPath);

        Instance = this;

        outputLog = new StreamWriter(outputLogPath, true);
        if (justCreated)
            outputLog.WriteLine("Level name;Time taken;Num of tries");
    }

    private void Start()
    {
        timer = Time.time;
        numOfTries = 0;
        currentLevel.CreateLevelFromText(levelFiles[indexOfCurrentLevel = 0]);
    }

    private void OnEnable()
    {
        currentLevel.OnWin += PlayWinSoundEffect;
        currentLevel.OnWin += RecordData;
        currentLevel.OnRestart += () => { numOfTries++; };
    }

    private void OnDisable()
    {
        currentLevel.OnWin -= PlayWinSoundEffect;
        currentLevel.OnWin -= RecordData;
        currentLevel.OnRestart -= () => { numOfTries++; };
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

    private void RecordData(bool hasWon)
    {
        if (!hasWon)
            numOfTries++;
        else
        {
            outputLog.WriteLine(NameOfCurrentLevel + ";" + (Time.time - timer) + ";" + numOfTries);
            timer = Time.time;
            numOfTries = 0;
        }
    }

    private void OnGUI()
    {
        float width = 175;
        float height = 100;
        GUI.Label(new Rect(Screen.width - width, Screen.height - height, width, height), "Level : " + (indexOfCurrentLevel + 1) + "/" + levelFiles.Length + " [" + levelFiles[indexOfCurrentLevel].name + "]\nPress 'n' to go to the next level\nPress 'b' to go back\nPress 'r' to restart");
    }

    private void OnApplicationQuit()
    {
        outputLog.Close();
    }
}
