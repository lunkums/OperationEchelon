using System;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    [SerializeField] private Troop troopPrefab;
    [SerializeField] private Transform troopContainer;
    [SerializeField] private Transform _transform;
    [SerializeField] private BoxCollider2D _collider;
    // Position data
    [Range(0f, 1f)][SerializeField] private float scale = 1f;
    [SerializeField] private float horizontalSpacing;
    [SerializeField] private float verticalSpacing;
    private Vector3 centerOffset;
    // Matrix data
    private Troop[,] currentLayout;
    private int rows;
    private int columns;
    // Maps Operations (enum) to their respective Formation move
    private Dictionary<Operation, MoveStrategy> operations;

    private float ScaledHorizontal => horizontalSpacing * scale;
    private float ScaledVertical => verticalSpacing * scale;

    public Troop[,] Troops => currentLayout;

    public event Action<Move> OnMoveAttempt;
    public delegate bool MoveStrategy(int[] selections);

    private void Awake()
    {
        operations = new Dictionary<Operation, MoveStrategy>();
        operations.Add(Operation.Convert, (selections) => Convert(selections));
        operations.Add(Operation.Promote, (selections) => Promote(selections));
        operations.Add(Operation.Demote, (selections) => Demote(selections));
        operations.Add(Operation.Swap, (selections) => Swap(selections));
        operations.Add(Operation.Attack, (selections) => Attack(selections));
    }

    public void Clear()
    {
        Destroy(troopContainer.gameObject);
        troopContainer = new GameObject("Troops").transform;
        troopContainer.parent = _transform;
        troopContainer.localPosition = Vector3.zero;
    }

    // Returns the row of the Formation given a position within it.
    public int SelectRow(Vector3 position)
    {
        float topEdgeYCoord = _transform.position.y  +  rows * ScaledHorizontal / 2;
        return (int)((topEdgeYCoord - position.y) / ScaledHorizontal);
    }

    public void ApplyMove(Move move)
    {
        move.Valid = operations[move.Operation].Invoke(move.Selections);
        OnMoveAttempt.Invoke(move);
    }

    // Sets the Formation using a 2D array (matrix).
    public void SetFromMatrix(int[,] matrix)
    {
        int i, j, rank;
        Troop troop;
        rows = matrix.GetLength(0);
        columns = matrix.GetLength(1);
        currentLayout = new Troop[rows, columns];

        for (i  = 0; i < rows; i++)
        {
            for (j = 0; j < columns; j++)
            {
                troop = Instantiate(troopPrefab);
                rank = matrix[i, j];
                troop.SetSignedRank(rank);
                troop.Scale = scale;
                troop.Parent = troopContainer.transform;
                currentLayout[i, j] = troop;
            }
        }
        CalculateOffset();
        RepositionTroops();
        ScaleCollider();
    }

    // Iterates through each formation and returns true if all Troops at each position are equal.
    public bool FormationEquals(Formation formation)
    {
        Troop[,] myTroops = Troops;
        Troop[,] theirTroops = formation.Troops;
        int i, rows = Math.Min(this.rows, theirTroops.GetLength(0));
        int j, columns = Math.Min(this.columns, theirTroops.GetLength(1));

        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < columns; j++)
            {
                if (myTroops[i, j].SignedRank != theirTroops[i, j].SignedRank)
                    return false;
            }
        }
        return true;
    }

    private void CalculateOffset()
    {
        centerOffset = new Vector3(ScaledHorizontal * (columns - 1) / 2, -ScaledVertical * (rows - 1) / 2);
    }

    // Repositions the Troops to align with the center of the Formation.
    private void RepositionTroops()
    {
        int i, j;
        Vector3 position;

        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < columns; j++)
            {
                position = new Vector3(ScaledHorizontal * j, -ScaledVertical * i) - centerOffset;
                currentLayout[i, j].Position = position;
            }
        }
    }

    // Scales the collider to surround the entire Formation.
    private void ScaleCollider()
    {
        _collider.size = new Vector2(columns * ScaledHorizontal, rows * ScaledVertical);
    }

    /*
     * Formation operations.
     */

    // Flips the sign of every Troop in the selected row.
    private bool Convert(int[] selections)
    {
        int j, row = selections[0];

        for (j = 0; j < columns; j++)
            currentLayout[row, j].FlipSign();

        return true;
    }

    // If no Troop in the selected row is a General, increase each Troop's rank and return true.
    private bool Promote(int[] selections)
    {
        bool isValid = true;
        int j, row = selections[0];

        // Validate the operation
        for (j = 0; isValid && j < columns; j++)
            isValid = currentLayout[row, j].Rank != Rank.General;
        // Apply the operation
        for (j = 0; isValid && j < columns; j++)
            currentLayout[row, j].Promote();

        return isValid;
    }

    // If no Troop in the selected row is a Private, decrease each Troop's rank and return true.
    private bool Demote(int[] selections)
    {
        bool isValid = true;
        int j, row = selections[0];

        // Validate the operation
        for (j = 0; isValid && j < columns; j++)
            isValid = currentLayout[row, j].Rank != Rank.Private;
        // Apply the operation
        for (j = 0; isValid && j < columns; j++)
            currentLayout[row, j].Demote();

        return isValid;
    }

    private bool Swap(int[] selections)
    {
        int j, firstRow = selections[0], secondRow = selections[1];
        int signedRank;

        for (j = 0; j < columns; j++)
        {
            signedRank = currentLayout[firstRow, j].SignedRank;
            currentLayout[firstRow, j].SetSignedRank(currentLayout[secondRow, j].SignedRank);
            currentLayout[secondRow, j].SetSignedRank(signedRank);
        }

        return true;
    }

    private bool Attack(int[] selections)
    {
        bool isValid = true;
        int j, firstRow = selections[0], secondRow = selections[1];
        Troop t1, t2;

        // Validate the operation
        for (j = 0; isValid && j < columns; j++)
        {
            t1 = currentLayout[firstRow, j];
            t2 = currentLayout[secondRow, j];
            isValid = t1.CanAdd(t2);
        }
        // Apply the operation
        for (j = 0; isValid && j < columns; j++)
            currentLayout[secondRow, j].Add(currentLayout[firstRow, j]);

        return isValid;
    }
}
