using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    public class ToArrayBenchmarks : BenchmarksBase
    {
        [BenchmarkCategory("Range")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Range() =>
            System.Linq.Enumerable.ToArray(linqRange);

        [BenchmarkCategory("LinkedList")]
        [Benchmark(Baseline = true)]
        public int[] Linq_LinkedList() =>
            System.Linq.Enumerable.ToArray(linkedList);

        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Array() =>
            System.Linq.Enumerable.ToArray(array);

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int[] Linq_List() =>
            System.Linq.Enumerable.ToArray(list);

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Enumerable_Reference() => 
            System.Linq.Enumerable.ToArray(enumerableReference);

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark(Baseline = true)]
        public int[] Linq_Enumerable_Value() => 
            System.Linq.Enumerable.ToArray(enumerableValue);

        [BenchmarkCategory("Range")]
        [Benchmark]
        public int[] Hyperlinq_Range() =>
            hyperlinqRange.ToArray();

        [BenchmarkCategory("LinkedList")]
        [Benchmark]
        public int[] Hyperlinq_LinkedList() =>
            linkedList.ToArray();

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int[] Hyperlinq_Array() =>
            array.ToArray();

        [BenchmarkCategory("List")]
        [Benchmark]
        public int[] Hyperlinq_List() =>
            list.ToArray();

        [BenchmarkCategory("Enumerable_Reference")]
        [Benchmark]
        public int[] Hyperlinq_Enumerable_Reference() => 
            enumerableReference
            .AsValueEnumerable()
            .ToArray();

        [BenchmarkCategory("Enumerable_Value")]
        [Benchmark]
        public int[] Hyperlinq_Enumerable_Value() => 
            enumerableValue
            .AsValueEnumerable<TestEnumerable.Enumerable, TestEnumerable.Enumerable.Enumerator, int>(enumerable => enumerable.GetEnumerator())
            .ToArray();
    }
}
