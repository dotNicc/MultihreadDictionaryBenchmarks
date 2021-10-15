using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace MultiThreadedDictionaryTester
{
    public class DictionaryBenchmarks
    {
        private readonly ConcurrentDictionary<int, int> concurrentDictionary = new ConcurrentDictionary<int, int>();
        private readonly ConcurrentDictionary<int, int> concurrentDictionaryFilled = new ConcurrentDictionary<int, int>();
        private readonly Dictionary<int, int> dictionary = new Dictionary<int, int>();
        private readonly Dictionary<int, int> dictionaryFilled;
        // private readonly List<int> keys = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        private readonly List<int> keys = new List<int> 
        {
            1,2,3,4,5,6,7,8,9,
            10,11,12,13,14,15,16,17,18,19,
            20,21,22,23,24,25,26,27,28,29,
            30,31,32,33,34,35,36,37,38,39,
            40,41,42,43,44,45,46,47,48,49,
            50,51,52,53,54,55,56,57,58,59,
            60,61,62,63,64,65,66,67,68,69,
            70,71,72,73,74,75,76,77,78,79,
            80,81,82,83,84,85,86,87,88,89,
            90,91,92,93,94,95,96,97,98,99,
            100
        };
        private readonly object sync = new object();

        // Initialize dictionaries from list
        public DictionaryBenchmarks()
        {
            this.keys.ForEach(x => this.concurrentDictionaryFilled.GetOrAdd(x, 1));
            this.dictionaryFilled = this.keys
                .Select((val, index) => new {Index = index + 1, Value = val})
                .ToDictionary(i => i.Index, i => i.Value);
        }
        
        [Benchmark]
        public void AddWithConcurrentDictionary()
        {
            Parallel.ForEach(this.keys, key =>
            {
                this.concurrentDictionary.GetOrAdd(key, 1);
            });
        }
        
        [Benchmark]
        public void AddWithLockedDictionary()
        {
            Parallel.ForEach(this.keys, key =>
            {
                lock (this.sync)
                {
                    if (!this.dictionary.TryGetValue(key, out var value))
                    {
                        this.dictionary.Add(key, 1);
                    }
                }
            });
        }
        
        [Benchmark]
        public void GetWithConcurrentDictionary()
        {
            Parallel.ForEach(this.keys, key =>
            {
                this.concurrentDictionaryFilled.GetOrAdd(key, 1);
            });
        }

        [Benchmark]
        public void GetWithLockedDictionary()
        {
            Parallel.ForEach(this.keys, key =>
            {
                lock (this.sync)
                {
                    if (!this.dictionaryFilled.TryGetValue(key, out var value))
                    {
                        throw new Exception("Not gonna happen.");
                    }
                }
            });
        }
    }
}