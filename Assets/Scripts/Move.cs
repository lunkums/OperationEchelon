public struct Move
{
    public Operation Operation { get; private set; }
    public int[] Selections { get; private set; }
    public bool Valid { get; set; }

    public Move(Operation operation, int[] selections)
    {
        Operation = operation;
        Selections = selections;
        Valid = false;
    }
}
