namespace ChronoECS.Core
{
    /// <summary>
    /// A unique identifier for an entity, including a generation counter
    /// to prevent stale-handle reuse.
    /// </summary>
    public struct Entity
    {
        /// <summary>Zero-based index into the internal arrays.</summary>
        public int Index { get; }

        /// <summary>
        /// Generation number; increments each time this index is recycled.
        /// </summary>
        public int Generation { get; }

        public Entity(int index, int generation)
        {
            Index = index;
            Generation = generation;
        }
    }
}