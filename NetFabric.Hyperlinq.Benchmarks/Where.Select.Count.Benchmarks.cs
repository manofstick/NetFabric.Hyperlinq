using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class WhereSelectCountBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public int Linq_Range() 
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(linqRange, _ => true), item => item));

        [BenchmarkCategory("LinkedList")]
        [Benchmark(Baseline = true)]
        public int Linq_LinkedList() 
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(linkedList, _ => true), item => item));

        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int Linq_Array() 
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, _ => true), item => item));

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int Linq_List() 
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(list, _ => true), item => item));

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark(Baseline = true)]
        public int Linq_Enumerable_Reference() 
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(enumerableReference, _ => true), item => item));

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark(Baseline = true)]
        public int Linq_Enumerable_Value()
            => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(enumerableValue, _ => true), item => item));

        [BenchmarkCategory("Range")]
        [Benchmark]
        public int Hyperlinq_Range() 
            => hyperlinqRange.Where(_ => true).Select(item => item).Count();

        [BenchmarkCategory("LinkedList")]
        [Benchmark]
        public int Hyperlinq_LinkedList() 
            => linkedList.Where(_ => true).Select(item => item).Count();

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int Hyperlinq_Array() 
            => array.Where(_ => true).Select(item => item).Count();

        [BenchmarkCategory("List")]
        [Benchmark]
        public int Hyperlinq_List() 
            => list.Where(_ => true).Select(item => item).Count();

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark]
        public int Hyperlinq_Enumerable_Reference()
            => enumerableReference
            .AsValueEnumerable()
            .Where(_ => true)
            .Select(item => item)
            .Count();

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark]
        public int Hyperlinq_Enumerable_Value()
            => enumerableValue
            .AsValueEnumerable<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>(enumerable => enumerable.GetEnumerator())
            .Where(_ => true)
            .Select(item => item)
            .Count();
    }
}
