using DiffMatchPatch;
using System.Collections.Generic;

namespace Dmp.Core
{
    /// <summary>
    /// Interface implemented by <see cref="Diff"/> to edit operations
    /// adapters.
    /// </summary>
    /// <typeparam name="TOperation">The type of the operation.</typeparam>
    public interface IEditOperationDiffAdapter<TOperation> where TOperation : class
    {
        /// <summary>
        /// Adapts the specified diffs list into a list of operations of type
        /// <see cref="TOperation"/>.
        /// </summary>
        /// <param name="diffs">The diffs.</param>
        /// <returns>The edit operations.</returns>
        IList<TOperation> Adapt(IList<Diff> diffs);
    }
}
