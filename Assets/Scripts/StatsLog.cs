using System.IO;
using UnityEngine;

public class StatsLog : MonoBehaviour
{
    private const string outputLogPath = "output.txt";

    [SerializeField] private Level level;

    private StreamWriter outputLog;
    private float timer;
    private int numOfTries;
    private bool openFile;

    private void Awake()
    {
        openFile = false;
    }

    private void Start()
    {
        ResetCurrentLevelStats();
        CreateOutputFile();
    }

    private void OnEnable()
    {
        level.OnWin += RecordData;
        level.OnRestart += () => { numOfTries++; };
    }

    private void OnDisable()
    {
        level.OnWin -= RecordData;
        level.OnRestart -= () => { numOfTries++; };
    }

    private void CreateOutputFile()
    {
        bool justCreated = !File.Exists(outputLogPath);

        outputLog = new StreamWriter(outputLogPath, true);
        if (justCreated)
            outputLog.WriteLine("Level name;Time taken;Num of tries");
        openFile = true;
    }

    private void RecordData(bool hasWon)
    {
        if (!hasWon)
            numOfTries++;
        else
            outputLog.WriteLine(LevelManager.Instance.NameOfCurrentLevel + ";" + (Time.time - timer) + ";" + numOfTries);
    }

    private void ResetCurrentLevelStats()
    {
        timer = Time.time;
        numOfTries = 0;
    }

    private void OnApplicationQuit()
    {
        if (openFile)
            outputLog.Close();
    }
}
