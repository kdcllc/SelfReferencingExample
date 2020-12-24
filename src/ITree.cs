using System.Collections.Generic;

namespace SelfReferencingSample
{
    /// <summary> Generic interface for tree node structure </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITree<T>
    {
        T Data { get; }
        ITree<T> Parent { get; }
        ICollection<ITree<T>> Children { get; }
        bool IsRoot { get; }
        bool IsLeaf { get; }
        int Level { get; }
    }
}
