using System;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Formation currentFormation;
    [SerializeField] private Formation optimalFormation;

    private string initialFormationStr;
    private string optimalFormationStr;
    private int totalMoves;
    private int movesLeft;
    private int[] allowableOperations;

    public int MovesLeft => movesLeft;
    public int[] AllowableOperations => allowableOperations;

    public event Action OnRestart;
    public event Action<bool> OnWin;

    private void OnEnable()
    {
        currentFormation.OnMoveAttempt += FormationMoveListener;
    }

    private void OnDisable()
    {
        currentFormation.OnMoveAttempt -= FormationMoveListener;
    }

    public void Restart()
    {
        // Clean up the old formation and create a new one
        ResetFormation(currentFormation, initialFormationStr);
        ResetFormation(optimalFormation, optimalFormationStr, true);
        // Reset level state
        movesLeft = totalMoves;
        OnRestart.Invoke();
    }

    // Sets the total moves, initial formation, and optimal formation from the given text file.
    public void CreateLevelFromText(TextAsset textFile)
    {
        string text = textFile.text;
        string[] words = text.Split(',');
        totalMoves = movesLeft = int.Parse(words[0]);
        allowableOperations = words[1].Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
        initialFormationStr = words[2].Trim();
        optimalFormationStr = words[3].Trim();
        Restart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        else if (Input.GetKeyDown(KeyCode.N))
            LevelManager.Instance.LoadNextLevel();
        else if (Input.GetKeyDown(KeyCode.B))
            LevelManager.Instance.LoadPreviousLevel();
    }

    private void FormationMoveListener(Move move)
    {
        if (!move.Valid)
            return;

        movesLeft--;

        if (currentFormation.FormationEquals(optimalFormation))
            OnWin.Invoke(true);
        else if (movesLeft == 0)
            OnWin.Invoke(false);
    }

    private void ResetFormation(Formation formation, string formationStr, bool scaled = false)
    {
        CreateFormationFromText(formation, formationStr, scaled);
    }

    /* Creates a Formation by parsing the text representation of a matrix into a 2D int,
       then loading it into the Formation. */
    private void CreateFormationFromText(Formation formation, string s, bool scaled = false)
    {
        int i = 0, j;
        string[] rows = s.Split('\n');
        int numOfColumns = rows[0].Count(f => (f == ' ')) + 1;
        int[,] result = new int[rows.Length, numOfColumns];

        foreach (string row in rows)
        {
            j = 0;
            foreach (string col in row.Trim().Split(' '))
            {
                result[i, j] = int.Parse(col.Trim());
                j++;
            }
            i++;
        }

        if (scaled)
            formation.Scale = 1 / (float)Math.Log(Math.Max(result.GetLength(0), result.GetLength(1)) + 1, 2);

        formation.SetFromMatrix(result);
    }
}
