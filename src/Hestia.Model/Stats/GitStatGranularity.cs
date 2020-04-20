namespace Hestia.Model.Stats
{
    public enum GitStatGranularity
    {
        /// <summary>
        /// Run line-level git stat analysis
        /// </summary>
        Full,

        /// <summary>
        /// Only run file-level git stat analysis
        /// </summary>
        File,
    }
}
