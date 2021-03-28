namespace AStar.Collections
{
    internal interface IModelAPriorityQueue<T>
    {
        int Push(T item);
        T Pop();
        T Peek();
        
        void Clear();
        int Count { get; }
    }
}
