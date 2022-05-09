using System.Collections.Generic;
using UnityEngine;

public class FormationGrid : MonoBehaviour
{
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private Transform _transform;

    private Vector3 bottomLeft;
    private Vector3 topRight;
    private int rows;
    private int columns;

    private List<GameObject> lines;

    private void Awake()
    {
        lines = new List<GameObject>();
    }

    public void Clear()
    {
        foreach (GameObject line in lines)
            Destroy(line);
        lines.Clear();
    }

    public void Create(Vector3 bottomLeft, Vector3 topRight, int rows, int columns, float scale)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        this.rows = rows;
        this.columns = columns;
        linePrefab.widthMultiplier = scale;
        CreateLines();
    }

    private void CreateLines()
    {
        Vector3 startPos, endPos, difference = topRight - bottomLeft;
        float width = difference.x;
        float height = difference.y;

        // Create the rows
        for (int i = 0; i < rows + 1; i++)
        {
            startPos = bottomLeft + new Vector3(0, i * height / rows);
            endPos = startPos + new Vector3(width, 0);
            CreateLine(startPos, endPos);
        }
        // Create the columns
        for (int i = 0; i < columns + 1; i++)
        {
            startPos = topRight - new Vector3(i * width / columns, 0);
            endPos = startPos - new Vector3(0, height);
            CreateLine(startPos, endPos);
        }
    }

    private void CreateLine(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer line = Instantiate(linePrefab, _transform);
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        lines.Add(line.gameObject);
    }
}
