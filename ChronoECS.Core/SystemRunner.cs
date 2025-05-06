using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Coordinates Awake and Update calls for all registered systems.
    /// </summary>
    public class SystemRunner
    {
        private readonly List<IAwakeSystem>  _awakeSystems  = new();
        private readonly List<IUpdateSystem> _updateSystems = new();

        public World World { get; private set; }

        private SystemRunner() { }

        /// <summary>
        /// Factory method to start runner for a given world.
        /// </summary>
        public static SystemRunner For(World world)
        {
            var runner = new SystemRunner { World = world };
            return runner;
        }

        /// <summary>
        /// Register an awake-system.
        /// </summary>
        public SystemRunner Add(IAwakeSystem sys)
        {
            _awakeSystems.Add(sys);
            return this;
        }

        /// <summary>
        /// Register an update-system.
        /// </summary>
        public SystemRunner Add(IUpdateSystem sys)
        {
            _updateSystems.Add(sys);
            return this;
        }

        /// <summary>
        /// Calls Awake on all IAwakeSystem instances.
        /// </summary>
        public SystemRunner Init()
        {
            foreach (var sys in _awakeSystems)
                sys.Awake(World);
            return this;
        }

        /// <summary>
        /// Calls Update on all IUpdateSystem instances.
        /// </summary>
        public void Update(float deltaTime)
        {
            foreach (var sys in _updateSystems)
                sys.Update(World, deltaTime);
        }
    }
}