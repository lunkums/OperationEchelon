using System.Collections.Generic;

public enum Operation
{
    Convert,
    Promote,
    Demote,
    March,
    Attack,
    None
}

public static class OperationExtensions
{
    private static Dictionary<Operation, int> selectionsNeeded = new Dictionary<Operation, int>()
    {
        { Operation.Convert, 1 },
        { Operation.Promote, 1 },
        { Operation.Demote, 1 },
        { Operation.March, 2 },
        { Operation.Attack, 2 },
        { Operation.None, 0 },
    };

    private static Dictionary<int, string> selectionText = new Dictionary<int, string>()
    {
        { 1, "Select a row to " },
        { 2, "Select 2 rows to " },
        { 0, "Select an operation" }
    };

    public static int SelectionsNeeded(this Operation operation)
    {
        return selectionsNeeded[operation];
    }

    public static string SelectionText(this Operation operation)
    {
        int selectionsNeeded = operation.SelectionsNeeded();
        string text = selectionText[selectionsNeeded];

        if (selectionsNeeded != 0)
            text += operation.ToString();
        return text;
    }
}