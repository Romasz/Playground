using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [EtwProfiler]
    public class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run<Program>();

        private Guid _guid;

        [GlobalSetup]
        public void Setup() => _guid = Guid.NewGuid();

        [Benchmark(Baseline = true)]
        public void Base64EncodedGuidMR()
        {
            for (int i = 0; i < 1000; i++)
                _guid.EncodeBase64StringMR();
        }

        [Benchmark]
        public void Base64EncodedGuidR()
        {
            for (int i = 0; i < 1000; i++)
                _guid.EncodeBase64StringR();
        }
    }
}
