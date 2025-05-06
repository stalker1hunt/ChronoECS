using System.Linq;

namespace ChronoECS.Core
{
    /// <summary>
    /// Moves all entities by their velocity each frame.
    /// </summary>
    public class MovementSystem : IAwakeSystem, IUpdateSystem
    {
        private EntityQuery<Position, Velocity> _query;
        
        public void Awake(World world)
        {
            world.RegisterStorage<Position>();
            world.RegisterStorage<Velocity>();
            
            _query = world.Query<Position, Velocity>();
        }

        public void Update(World world, float deltaTime)
        {
            var posStorage = world.GetStorage<Position>();
            foreach (var (entity, pos, vel) in _query)
            {
                var updatedPos = pos;

                updatedPos.X += vel.VX * deltaTime;
                updatedPos.Y += vel.VY * deltaTime;

                posStorage.Add(entity.Index, updatedPos);
            }
        }
    }
}