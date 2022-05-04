using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextAsset levelTextFile;
    [SerializeField] private Formation initialFormation;
    [SerializeField] private Formation optimalFormation;
    [SerializeField] private GameObject buttonController;

    private int movesLeft;

    public int MovesLeft => movesLeft;

    private void Start()
    {
        CreateLevelFromText();
    }

    private void OnEnable()
    {
        initialFormation.OnMoveAttempt += FormationMoveListener;
    }

    private void OnDisable()
    {
        initialFormation.OnMoveAttempt -= FormationMoveListener;
    }

    private void Update()
    {
        if (movesLeft == 0)
        {
            buttonController.SetActive(false);

            if (initialFormation.FormationEquals(optimalFormation))
                Debug.Log("Win!");
            else
                Debug.Log("Lose!");
        }
    }

    private void FormationMoveListener(Move move)
    {
        if (move.Valid)
            movesLeft--;
    }

    private void CreateLevelFromText()
    {
        string text = levelTextFile.text;
        string[] words = text.Split(',');
        movesLeft = int.Parse(words[0]);
        CreateFormationFromText(initialFormation, words[1].Trim());
        CreateFormationFromText(optimalFormation, words[2].Trim());
    }

    private void CreateFormationFromText(Formation formation, string s)
    {
        int i = 0, j;
        string[] rows = s.Split('\n');
        int numOfColumns = rows[0].Count(f => (f == ' ')) + 1;
        int[,] result = new int[rows.Length, numOfColumns];
        Debug.Log("rows : " + rows.Length +  ", num cols : " + numOfColumns);

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
