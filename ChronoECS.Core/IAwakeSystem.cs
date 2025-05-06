namespace ChronoECS.Core
{
    /// <summary>
    /// Logic that runs once at startup (e.g. filter setup).
    /// </summary>
    public interface IAwakeSystem
    {
        void Awake(World world);
    }
}