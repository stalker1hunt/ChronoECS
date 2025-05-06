namespace ChronoECS.Core
{
    /// <summary>
    /// Logic that runs each frame/update.
    /// </summary>
    public interface IUpdateSystem
    {
        void Update(World world, float deltaTime);
    }
}