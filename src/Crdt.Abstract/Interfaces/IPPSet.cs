namespace Crdt.Abstract.Interfaces
{
    public interface IPPSet<T> : ISet<T>
    {
        void Remove(T element);

        IPPSet<T> Merge(IPPSet<T> set);

        ISet<T> AddSet { get; }

        ISet<T> RemoveSet { get; }
    }
}
