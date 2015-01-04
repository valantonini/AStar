namespace AStar
{
    public interface IPriorityQueue<T>
    {
        int Push(T item);
        T Pop();
        T Peek();
        
        void Clear();
        int Count { get; }
    }
}
