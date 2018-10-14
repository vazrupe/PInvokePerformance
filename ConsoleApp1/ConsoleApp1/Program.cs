using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        [DllImport("example.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp(double[] arr1, double[] arr2, int length);
        [DllImport("example.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp2(int num);

        [DllImport("example.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCppUnman(int length);
        [DllImport("example.dll")]
        [SuppressUnmanagedCodeSecurity]
        private static extern double ComplexCpp2Unman(int num);

        public static double GetRunningTime(ulong count, Action<ulong> action)
        {
            return GetRunningTime(() => action?.Invoke(count));
        }

        public static double GetRunningTime(Action action)
        {
            var sw = new Stopwatch();

            sw.Start();
            action?.Invoke();
            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }

        public static double ComplexCsharp(double[] arr1, double[] arr2, int length)
        {
            double r = 0.0;
            for (int i = 0; i < length; i++)
            {
                r = r + arr1[i] * arr2[i];
                r = r - arr1[i] * arr2[length - i - 1];
                r = Math.Sqrt(r);
            }
            return r;
        }
        public static double ComplexCsharp2(int num)
        {
            double n = num;

            while (n > 1.0)
            {
                n = n / 1.00000001;
            }
            return n;
        }

        static void Main(string[] args)
        {
            var arr2 = Enumerable.Range(0, 10000000).Select(n => (double)n).ToArray();
            var timeCpp1 = GetRunningTime(() => ComplexCpp(arr2, arr2, arr2.Length));
            var timeCppUnman1 = GetRunningTime(() => ComplexCppUnman(arr2.Length));
            var timeCSharp1 = GetRunningTime(() => ComplexCsharp(arr2, arr2, arr2.Length));

            Console.WriteLine($"C extension: {timeCpp1}ms, {timeCppUnman1}ms / C#: {timeCSharp1}ms");

            var targetNum = 1000000000;
            var timeCpp2 = GetRunningTime(() => ComplexCpp2(targetNum));
            var timeCppUnman2 = GetRunningTime(() => ComplexCpp2Unman(targetNum));
            var timeCSharp2 = GetRunningTime(() => ComplexCsharp2(targetNum));
            
            Console.WriteLine($"C extension: {timeCpp2}ms, {timeCppUnman2}ms / C#: {timeCSharp2}ms");
        }
    }
}
