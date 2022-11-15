
namespace _Main.Scripts.GamePlay.HealthSystem
{
    [System.Flags]
    public enum DamageDealerType
    {
        Player = 1,
        AI = 1 << 1,
        Environment = 1 << 2,
    } 
}
