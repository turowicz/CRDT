using System;
using System.Linq;

namespace Crdt.Core
{
    public class Counter : ICounter
    {
        readonly Int32 _id;
        readonly Int32 _nodes;
        readonly Int64[] _payload;

        public Counter(Int32 id, Int32 nodes)
        {
            _id = id;
            _nodes = nodes;
            _payload = new Int64[nodes];
        }

        public void Increment()
        {
            _payload[_id] += 1;
        }

        public Int64 Value
        {
            get { return _payload.Sum(x => x); }
        }

        public void Merge(ICounter counter)
        {
            if (counter == null)
            {
                throw new ArgumentNullException(nameof(counter));
            }

            for (var i = 0; i < _nodes; i++)
            {
                _payload[i] = Math.Max(_payload[i], counter[i]);
            }
        }

        public Int32 CompareTo(object obj)
        {
            var counter = obj as ICounter;

            if (counter == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            for (var i = 0; i < _nodes; i++)
            {
                if (_payload[i] > counter[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        public Int64 this[Int32 i]
        {
            get
            {
                return _payload[i];
            }
            set { throw new InvalidOperationException("Cannot set externally."); }
        }
    }
}
