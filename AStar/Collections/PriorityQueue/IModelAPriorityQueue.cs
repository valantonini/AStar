namespace AStar.Collections.PriorityQueue
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
