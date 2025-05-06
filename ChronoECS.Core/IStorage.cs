using System.Collections.Generic;

namespace ChronoECS.Core
{
    /// <summary>
    /// Non‐generic interface to a component storage, used for filtering.
    /// </summary>
    public interface IStorage
    {
        /// <summary> How many entities have this component. </summary>
        int Count { get; }

        /// <summary> All stored entity indices. </summary>
        IEnumerable<int> EntityIndices { get; }

        /// <summary> True if the given entity index has this component. </summary>
        bool Has(int entityIndex);
    }
}