using System.Collections.Generic;

namespace AStar.Collections.PriorityQueue
{
    internal class SimplePriorityQueue<T> : IModelAPriorityQueue<T>
    {
        private readonly List<T> _innerList = new List<T>();
        private readonly IComparer<T> _comparer;

        public SimplePriorityQueue(IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;;
        }

        public T Peek()
        {
            return _innerList.Count > 0 ? _innerList[0] : default(T);
        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public int Push(T item)
        {
            var p = _innerList.Count;
            _innerList.Add(item); // E[p] = O

            do
            {
                if (p == 0)
                {
                    break;
                }
                
                var p2 = (p - 1) / 2;

                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break;
                }

            } while (true);

            return p;
        }

        public T Pop()
        {
            var result = _innerList[0];
            var p = 0;

            _innerList[0] = _innerList[_innerList.Count - 1];
            _innerList.RemoveAt(_innerList.Count - 1);

            do
            {
                var pn = p;
                var p1 = 2 * p + 1;
                var p2 = 2 * p + 2;

                if (_innerList.Count > p1 && OnCompare(p, p1) > 0)
                {
                    p = p1;
                }
                if (_innerList.Count > p2 && OnCompare(p, p2) > 0)
                {
                    p = p2;
                }

                if (p == pn)
                {
                    break;
                }

                SwitchElements(p, pn);

            } while (true);

            return result;
        }

        public T this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                _innerList[index] = value;
                Update(index);
            }
        }

        private void Update(int i)
        {
            var p = i;
            int p2;

            do	
            {
                if (p == 0)
                {
                    break;
                }

                p2 = (p - 1) / 2;

                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                {
                    break;
                }

            } while (true);

            if (p < i)
            {
                return;
            }

            do
            {
                var pn = p;
                var p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                
                if (_innerList.Count > p1 && OnCompare(p, p1) > 0) 
                {
                    p = p1;
                }
                
                if (_innerList.Count > p2 && OnCompare(p, p2) > 0)
                {
                    p = p2;
                }

                if (p == pn)
                {
                    break;
                }
                
                SwitchElements(p, pn);

            } while (true);
        }

        private void SwitchElements(int i, int j)
        {
            var h = _innerList[i];
            _innerList[i] = _innerList[j];
            _innerList[j] = h;
        }

        private int OnCompare(int i, int j)
        {
            return _comparer.Compare(_innerList[i], _innerList[j]);
        }
    }
}
