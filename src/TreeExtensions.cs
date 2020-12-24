using System;
using System.Collections.Generic;
using System.Linq;

namespace SelfReferencingSample
{
    public static class TreeExtensions
    {
        /// <summary> Flatten tree to plain list of nodes </summary>
        public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            return nodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(nodes);
        }

        /// <summary> Converts given list to tree. </summary>
        /// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        /// <param name="items">The collection items.</param>
        /// <param name="parentSelector">Expression to select parent.</param>
        public static ITree<T> ToTree<T>(this IList<T> items, Func<T, T, bool> parentSelector)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
                child => child);
            return Tree<T>.FromLookup(lookup);
        }

        public static List<T> GetParents<T>(this ITree<T> node, List<T> parentNodes = null) where T : class
        {
            while (true)
            {
                parentNodes ??= new List<T>();
                if (node?.Parent?.Data == null) return parentNodes;
                parentNodes.Add(node.Parent.Data);
                node = node.Parent;
            }
        }
    }
}
