public enum Rank
{
    Empty = 0,
    Private = 1,
    Sergeant = 2,
    Captain = 4,
    General = 8
}

public static class RankExtensions
{
    public static int Next(this Rank rank)
    {
        return (int)rank << 1 % 8;
    }

    public static int Previous(this Rank rank)
    {
        return ((int)rank + 1) >> 1;
    }
}