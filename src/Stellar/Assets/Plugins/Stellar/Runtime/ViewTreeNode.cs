using System.Collections.Generic;

namespace Plugins.Stellar.Runtime
{
    public class ViewTreeNode<T>
    {
        public T Value { get; set; }
        public List<ViewTreeNode<T>> Children { get; set; }

        public ViewTreeNode(T value)
        {
            Value = value;
            Children = new List<ViewTreeNode<T>>();
        }

        public void AddChild(ViewTreeNode<T> child)
        {
            Children.Add(child);
        }

        public void RemoveChild(ViewTreeNode<T> child)
        {
            Children.Remove(child);
        }
    }
    
    public class ViewTree<T>
    {
        public ViewTreeNode<T> Root { get; set; }

        public ViewTree(T rootValue)
        {
            Root = new ViewTreeNode<T>(rootValue);
        }

        
    }

}