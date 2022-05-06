using System;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextAsset levelTextFile;
    [SerializeField] private Formation currentFormation;
    [SerializeField] private Formation optimalFormation;

    private int totalMoves;
    private int movesLeft;
    private string initialFormationStr;

    public int MovesLeft => movesLeft;

    public event Action OnRestart;
    public event Action<bool> OnWin;

    private void Start()
    {
        CreateLevelFromText();
        Restart();
    }

    private void OnEnable()
    {
        currentFormation.OnMoveAttempt += FormationMoveListener;
        OnWin += (hasWon) => Debug.Log("Has won? " + hasWon);
    }

    private void OnDisable()
    {
        currentFormation.OnMoveAttempt -= FormationMoveListener;
    }

    public void Restart()
    {
        // Clean up the old formation and create a new one
        ResetCurrentFormation();
        // Reset level state
        movesLeft = totalMoves;
        OnRestart.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
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

    private void ResetCurrentFormation()
    {
        currentFormation.Clear();
        CreateFormationFromText(currentFormation, initialFormationStr);
    }

    // Sets the total moves, initial formation, and optimal formation from the given text file.
    private void CreateLevelFromText()
    {
        string text = levelTextFile.text;
        string[] words = text.Split(',');
        totalMoves = movesLeft = int.Parse(words[0]);
        initialFormationStr = words[1].Trim();
        CreateFormationFromText(optimalFormation, words[2].Trim());
    }

    /* Creates a Formation by parsing the text representation of a matrix into a 2D int,
       then loading it into the Formation. */
    private void CreateFormationFromText(Formation formation, string s)
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
        formation.SetFromMatrix(result);
    }
}
