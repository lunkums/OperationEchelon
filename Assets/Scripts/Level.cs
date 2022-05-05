using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextAsset levelTextFile;
    [SerializeField] private Formation mainFormationTemplate;
    [SerializeField] private Formation optimalFormation;
    [SerializeField] private GameObject buttonController;

    private Formation currentFormation;
    private int totalMoves;
    private int movesLeft;
    private string initialFormationStr;

    public int MovesLeft => movesLeft;

    private void Start()
    {
        // currentFormation is instantiated once to avoid a null check when deleting it on reset
        mainFormationTemplate.Interactable = false;
        currentFormation = Instantiate(mainFormationTemplate);
        CreateLevelFromText();
        Restart();
    }

    public void Restart()
    {
        // Clean up the old formation and create a new one
        ResetCurrentFormation();
        // Reset level state
        movesLeft = totalMoves;
        buttonController.SetActive(true);
    }

    private void Update()
    {
        if (movesLeft == 0)
        {
            buttonController.SetActive(false);

            if (currentFormation.FormationEquals(optimalFormation))
                Debug.Log("Win!");
            else
                Debug.Log("Lose!");
        }

        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    private void FormationMoveListener(Move move)
    {
        if (move.Valid)
            movesLeft--;
    }

    private void ResetCurrentFormation()
    {
        Destroy(currentFormation.gameObject);
        currentFormation = Instantiate(mainFormationTemplate, mainFormationTemplate.Position, mainFormationTemplate.Rotation);
        currentFormation.OnMoveAttempt += FormationMoveListener;
        currentFormation.Interactable = true;
        currentFormation.name = "Current Formation";
        CreateFormationFromText(currentFormation, initialFormationStr);
    }

    private void CreateLevelFromText()
    {
        string text = levelTextFile.text;
        string[] words = text.Split(',');
        totalMoves = movesLeft = int.Parse(words[0]);
        initialFormationStr = words[1].Trim();
        CreateFormationFromText(optimalFormation, words[2].Trim());
    }

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
