using System.Linq;
using Crdt.Core;
using Machine.Fakes;
using Machine.Specifications;
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Crdt.Tests
{
    [Subject(typeof(Counter))]
    public abstract class BaseCounterTest : WithSubject<Counter>
    {
        protected const int N = 100;
    }

    public class When_incrementing_once : BaseCounterTest
    {
        Because of = () => Subject.Increment();

        It should_return_1 = () => Subject.Value.ShouldEqual(1);
    }

    public class When_incrementing_n_times : BaseCounterTest
    {
        Because of = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());

        It should_return_n = () => Subject.Value.ShouldEqual(N);
    }

    public class When_merging_smaller_to_bigger_counters : BaseCounterTest
    {
        static ICounter _target;
        static ICounter _merged;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _merged = Subject.Merge(_target);

        It should_return_sum_of_both_as_value = () => _merged.Value.ShouldEqual(N + N / 2);
    }

    public class When_merging_bigger_to_smaller_counters : BaseCounterTest
    {
        static ICounter _target;
        static ICounter _merged;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N / 2).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _merged = Subject.Merge(_target);

        It should_return_sum_of_both_as_value = () => _merged.Value.ShouldEqual(N + N / 2);
    }

    public class When_merging_equal_to_equal_counters : BaseCounterTest
    {
        static ICounter _target;
        static ICounter _merged;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _merged = Subject.Merge(_target);

        It should_return_sum_of_both_as_value = () => _merged.Value.ShouldEqual(N * 2);
    }

    public class When_comparing_smaller_to_bigger_counters : BaseCounterTest
    {
        static ICounter _target;
        static int _comparison;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N / 2).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _comparison = Subject.CompareTo(_target);

        It should_return_negative_1 = () => _comparison.ShouldEqual(-1);
    }

    public class When_comparing_bigger_to_smaller_counters : BaseCounterTest
    {
        static ICounter _target;
        static int _comparison;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _comparison = Subject.CompareTo(_target);

        It should_return_1 = () => _comparison.ShouldEqual(1);
    }

    public class When_comparing_equal_to_equal_counters : BaseCounterTest
    {
        static ICounter _target;
        static int _comparison;

        Establish that = () =>
        {
            _target = new Counter();
            Enumerable.Range(0, N).ToList().ForEach(x => _target.Increment());
            Enumerable.Range(0, N).ToList().ForEach(x => Subject.Increment());
        };

        Because of = () => _comparison = Subject.CompareTo(_target);

        It should_return_0 = () => _comparison.ShouldEqual(0);
    }
}
