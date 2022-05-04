using System;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    [SerializeField] private Troop troopPrefab;
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

    private Dictionary<Operation, Action<int[]>> operations;

    private float ScaledHorizontal => horizontalSpacing * scale;
    private float ScaledVertical => verticalSpacing * scale;

    public Troop[,] Troops => currentLayout;

    public event Action OnMove;

    private void Awake()
    {
        operations = new Dictionary<Operation, Action<int[]>>();
        operations.Add(Operation.Convert, (selections) => Convert(selections));
        operations.Add(Operation.Promote, (selections) => Promote(selections));
        operations.Add(Operation.Demote, (selections) => Demote(selections));
        operations.Add(Operation.March, (selections) => March(selections));
        operations.Add(Operation.Attack, (selections) => Attack(selections));
    }

    // Returns the row of the Formation given a position within it.
    public int SelectRow(Vector3 position)
    {
        float topEdgeYCoord = _transform.position.y  +  rows * ScaledHorizontal / 2;
        return (int)((topEdgeYCoord - position.y) / ScaledHorizontal);
    }

    public void ApplyOperation(Operation currentOperation, int[] selections)
    {
        operations[currentOperation].Invoke(selections);
        OnMove.Invoke();
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
                troop.SetRank(rank);
                troop.Scale = scale;
                troop.Parent = _transform;
                currentLayout[i, j] = troop;
            }
        }
        CalculateOffset();
        RepositionTroops();
        ScaleCollider();
    }

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
                if (!myTroops[i, j].TroopEquals(theirTroops[i, j]))
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

    private void Convert(int[] selections)
    {
        int j, row = selections[0];

        for (j = 0; j < columns; j++)
            currentLayout[row, j].FlipSign();
    }

    private void Promote(int[] selections)
    {

    }

    private void Demote(int[] selections)
    {

    }

    private void March(int[] selections)
    {

    }

    private void Attack(int[] selections)
    {

    }
}
