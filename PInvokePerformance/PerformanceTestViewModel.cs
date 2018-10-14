using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PInvokePerformance
{
    public class PerformanceTestViewModel : RunTestViewModelBase
    {
        private int _repeat = 50;
        public int Repeat
        {
            get { return _repeat; }
            set
            {
                var val = value;
                if (val < 1)
                    val = 1;

                SetProperty(ref _repeat, val);
            }
        }

        private ulong _iterations = 1000000UL;
        public ulong Iterations
        {
            get { return _iterations; }
            set
            {
                var val = value;
                if (val < 1)
                    val = 1;

                SetProperty(ref _iterations, val);
            }
        }

        protected override IEnumerable<Action> GenerateJobs()
        {
            var test = new PerformanceTest(Repeat, Iterations);

            var props = typeof(PerformanceTestViewModel).GetProperties()
                .Where(p => p.Name.EndsWith("Result") && p.CanWrite && p.PropertyType == typeof(double));

            foreach (var prop in props)
                prop.SetValue(this, 0.0);

            // Run C# safe code
            yield return () => CSharpSafeTest1Result = test.BenchmarkCSharpSafeTest1();
            yield return () => CSharpSafeTest2Result = test.BenchmarkCSharpSafeTest2();
            yield return () => CSharpSafeTest3Result = test.BenchmarkCSharpSafeTest3();

            // Run C# unsafe code
            yield return () => CSharpUnsafeTest1Result = test.BenchmarkCSharpUnsafeTest1();
            yield return () => CSharpUnsafeTest2Result = test.BenchmarkCSharpUnsafeTest2();
            yield return () => CSharpUnsafeTest3Result = test.BenchmarkCSharpUnsafeTest3();

            // Run the unmanged tests.
            yield return () => UnmanagedTest1Result = test.BenchmarkUnmanagedTest1();
            yield return () => UnmanagedTest2Result = test.BenchmarkUnmanagedTest2();
            yield return () => UnmanagedTest3Result = test.BenchmarkUnmanagedTest3();

            // Run the tests through the interface.
            yield return () => ManagedInterfaceTest1Result = test.BenchmarkManagedInterfaceTest1();
            yield return () => ManagedInterfaceTest2Result = test.BenchmarkManagedInterfaceTest2();
            yield return () => ManagedInterfaceTest3Result = test.BenchmarkManagedInterfaceTest3();

            // Run the tests through the pinvoke.
            yield return () => PInvokeTest1Result = test.BenchmarkPInvokeTest1();
            yield return () => PInvokeTest2Result = test.BenchmarkPInvokeTest2();
            yield return () => PInvokeTest3Result = test.BenchmarkPInvokeTest3();

            // Run the tests through the unmanaged pinvoke.
            yield return () => UnsafePInvokeTest1Result = test.BenchmarkUnsafePInvokeTest1();
            yield return () => UnsafePInvokeTest2Result = test.BenchmarkUnsafePInvokeTest2();
            yield return () => UnsafePInvokeTest3Result = test.BenchmarkUnsafePInvokeTest3();
            
            yield return () => UnsafePInvokeTest3CallOnlyResult = test.BenchmarkUnsafePInvokeTest3CallOnly();
        }

        #region Test Result Props
        private double cSharpSafeTest1Result;
        private double cSharpSafeTest2Result;
        private double cSharpSafeTest3Result;
        public double CSharpSafeTest1Result
        {
            get { return cSharpSafeTest1Result; }
            set { SetProperty(ref cSharpSafeTest1Result, value); }
        }
        public double CSharpSafeTest2Result
        {
            get { return cSharpSafeTest2Result; }
            set { SetProperty(ref cSharpSafeTest2Result, value); }
        }
        public double CSharpSafeTest3Result
        {
            get { return cSharpSafeTest3Result; }
            set { SetProperty(ref cSharpSafeTest3Result, value); }
        }

        private double cSharpUnsafeTest1Result;
        private double cSharpUnsafeTest2Result;
        private double cSharpUnsafeTest3Result;
        public double CSharpUnsafeTest1Result
        {
            get { return cSharpUnsafeTest1Result; }
            set { SetProperty(ref cSharpUnsafeTest1Result, value); }
        }
        public double CSharpUnsafeTest2Result
        {
            get { return cSharpUnsafeTest2Result; }
            set { SetProperty(ref cSharpUnsafeTest2Result, value); }
        }
        public double CSharpUnsafeTest3Result
        {
            get { return cSharpUnsafeTest3Result; }
            set { SetProperty(ref cSharpUnsafeTest3Result, value); }
        }

        private double unmanagedTest1Result;
        private double unmanagedTest2Result;
        private double unmanagedTest3Result;
        public double UnmanagedTest1Result
        {
            get { return unmanagedTest1Result; }
            set { SetProperty(ref unmanagedTest1Result, value); }
        }
        public double UnmanagedTest2Result
        {
            get { return unmanagedTest2Result; }
            set { SetProperty(ref unmanagedTest2Result, value); }
        }
        public double UnmanagedTest3Result
        {
            get { return unmanagedTest3Result; }
            set { SetProperty(ref unmanagedTest3Result, value); }
        }

        private double _managedInterfaceTest1Result;
        private double _managedInterfaceTest2Result;
        private double _managedInterfaceTest3Result;
        public double ManagedInterfaceTest1Result
        {
            get { return _managedInterfaceTest1Result; }
            set { SetProperty(ref _managedInterfaceTest1Result, value); }
        }
        public double ManagedInterfaceTest2Result
        {
            get { return _managedInterfaceTest2Result; }
            set { SetProperty(ref _managedInterfaceTest2Result, value); }
        }
        public double ManagedInterfaceTest3Result
        {
            get { return _managedInterfaceTest3Result; }
            set { SetProperty(ref _managedInterfaceTest3Result, value); }
        }

        private double pInvokeTest1Result;
        private double pInvokeTest2Result;
        private double pInvokeTest3Result;
        public double PInvokeTest1Result
        {
            get { return pInvokeTest1Result; }
            set { SetProperty(ref pInvokeTest1Result, value); }
        }
        public double PInvokeTest2Result
        {
            get { return pInvokeTest2Result; }
            set { SetProperty(ref pInvokeTest2Result, value); }
        }
        public double PInvokeTest3Result
        {
            get { return pInvokeTest3Result; }
            set { SetProperty(ref pInvokeTest3Result, value); }
        }

        private double unsafePInvokeTest1Result;
        private double unsafePInvokeTest2Result;
        private double unsafePInvokeTest3Result;
        public double UnsafePInvokeTest1Result
        {
            get { return unsafePInvokeTest1Result; }
            set { SetProperty(ref unsafePInvokeTest1Result, value); }
        }
        public double UnsafePInvokeTest2Result
        {
            get { return unsafePInvokeTest2Result; }
            set { SetProperty(ref unsafePInvokeTest2Result, value); }
        }
        public double UnsafePInvokeTest3Result
        {
            get { return unsafePInvokeTest3Result; }
            set { SetProperty(ref unsafePInvokeTest3Result, value); }
        }


        private double unsafePInvokeTest3CallOnlyResult;
        public double UnsafePInvokeTest3CallOnlyResult
        {
            get { return unsafePInvokeTest3CallOnlyResult; }
            set { SetProperty(ref unsafePInvokeTest3CallOnlyResult, value); }
        }
        #endregion
    }
}
