using System;
using System.Linq;
using Crdt.Abstract.Interfaces;
using Crdt.Core.Sets;
using Machine.Fakes;
using Machine.Specifications;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeTypeMemberModifiers

namespace Crdt.Tests.Sets
{
    public class SetTests
    {
        [Subject(typeof(Set<Int32>))]
        public abstract class BaseSetTest : WithSubject<Set<Int32>>
        {
            protected const Int32 N = 100;
        }

        public class When_adding_an_item : BaseSetTest
        {
            Because of = () => Subject.Add(N);

            It should_contain_only_N = () => Subject.Single().ShouldEqual(N);
        }

        public class When_adding_N_items : BaseSetTest
        {
            Because of = () => Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));

            It should_contain_N_elements = () => Subject.LongCount().ShouldEqual(N);
        }

        public class When_merging_smaller_to_bigger_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static ISet<Int32> _merged;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N + N / 2);

            It should_contain_target = () => _merged.ShouldContain(_target);

            It should_contain_source = () => _merged.ShouldContain(Subject);
        }

        public class When_merging_bigger_to_smaller_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static ISet<Int32> _merged;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N / 2).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N + N / 2);

            It should_contain_target = () => _merged.ShouldContain(_target);

            It should_contain_source = () => _merged.ShouldContain(Subject);
        }

        public class When_merging_equal_to_equal_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static ISet<Int32> _merged;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(N, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _merged = Subject.Merge(_target);

            It should_return_sum_of_both_as_value = () => _merged.LongCount().ShouldEqual(N * 2);

            It should_contain_target = () => _merged.ShouldContain(_target);

            It should_contain_source = () => _merged.ShouldContain(Subject);
        }

        public class When_comparing_smaller_to_bigger_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N / 2).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_not_return_0 = () => _comparison.ShouldNotEqual(0);
        }

        public class When_comparing_bigger_to_smaller_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N / 2).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_return_0 = () => _comparison.ShouldEqual(0);
        }

        public class When_comparing_equal_to_equal_sets : BaseSetTest
        {
            static ISet<Int32> _target;
            static Int32 _comparison;

            Establish that = () =>
            {
                _target = new Set<Int32>();
                Enumerable.Range(0, N).ToList().ForEach(x => _target.Add(x));
                Enumerable.Range(0, N).ToList().ForEach(x => Subject.Add(x));
            };

            Because of = () => _comparison = Subject.CompareTo(_target);

            It should_return_0 = () => _comparison.ShouldEqual(0);
        }
    }
}
