using System.Collections.Generic;

public enum Operation
{
    Convert,
    Swap,
    Promote,
    Demote,
    Reinforce,
    None
}

public static class OperationExtensions
{
    private static Dictionary<Operation, int> selectionsNeeded = new Dictionary<Operation, int>()
    {
        { Operation.Convert, 1 },
        { Operation.Promote, 1 },
        { Operation.Demote, 1 },
        { Operation.Swap, 2 },
        { Operation.Reinforce, 2 },
        { Operation.None, 0 },
    };

    private static string[] selectionText = new string[]{ "Select an operation", "Select a row to ", "Select 2 rows to " };

    private static string[] selectionSuffix = new string[] { "", "\n(hover to preview)", "" };

    public static int SelectionsNeeded(this Operation operation)
    {
        return selectionsNeeded[operation];
    }

    public static string SelectionText(this Operation operation)
    {
        int selectionsNeeded = operation.SelectionsNeeded();
        string text = selectionText[selectionsNeeded];

        if (selectionsNeeded != 0)
            text += operation.ToString() + selectionSuffix[selectionsNeeded];
        return text;
    }
}