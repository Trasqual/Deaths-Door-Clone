
[System.Flags]
public enum DamageDealerType
{
    None = 0,
    Player = 1,
    AI = 1 << 1,
    Environment = 1 << 2,
}
