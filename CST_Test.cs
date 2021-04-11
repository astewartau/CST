using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace CST
{
    /// <summary>
    /// An assertion exception for unit testing purposes
    /// </summary>
    public class AssertionException : ApplicationException
    {
        public AssertionException(string message = "") : base(message) { }
    }

    /// <summary>
    /// Contains assertions for collections
    /// </summary>
    public static class CollectionAssert
    {
        /// <summary>
        /// Throws an AssertionException if the given collections do not contain equivalent elements
        /// </summary>
        /// <param name="expected">The collection containing the expected values</param>
        /// <param name="actual">The collection containing the actual values</param>
        /// <param name="message">The exception message to use if the collection elements are not equal</param>
        /// <typeparam name="T">The contained type of each collection</typeparam>
        public static void AreEqual<T>(T[] expected, T[] actual, string message = "")
        {
            if (expected.Length != actual.Length) throw new AssertionException($"Expected length of {expected.Length} but got {actual.Length}");
            for (int i = 0; i < expected.Length; i++)
            {
                if (!expected[i].Equals(actual[i]))
                {
                    if (message != "") throw new AssertionException(message);
                    else throw new AssertionException($"Expected {expected[i]} at i={i} but got {actual[i]}");
                }
            }
        }

        /// <summary>
        /// Throws an AssertionException if the given collections do not contain equivalent elements
        /// </summary>
        /// <param name="expected">The collection containing the expected values</param>
        /// <param name="actual">The collection containing the actual values</param>
        /// <param name="message">The exception message to use if the collection elements are not equal</param>
        /// <typeparam name="T">The contained type of each collection</typeparam>
        public static void AreEqual<T>(T[,] expected, T[,] actual, string message = "")
        {
            if (expected.GetLength(0) != actual.GetLength(0) || expected.GetLength(1) != actual.GetLength(1))
            {
                throw new AssertionException($"Expected dimensions of {expected.GetLength(0)}x{expected.GetLength(1)} but got {actual.GetLength(0)}x{actual.GetLength(1)}");
            }
            
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    if (!expected[i, j].Equals(actual[i, j]))
                    {
                        if (message != "") throw new AssertionException(message);
                        else throw new AssertionException($"Expected {expected[i,j]} at {i},{j} but got {actual[i,j]}");
                    }
                }
            }
        }

        /// <summary>
        /// Throws an AssertionException if the given collections do not contain equivalent elements
        /// </summary>
        /// <param name="expected">The collection containing the expected values</param>
        /// <param name="actual">The collection containing the actual values</param>
        /// <param name="message">The exception message to use if the collection elements are not equal</param>
        /// <typeparam name="T">The contained type of each collection</typeparam>
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message = "")
        {
            if (message == "") message = $"Expected {string.Join(",", expected)} but got {string.Join(",", actual)}";
            if (!expected.SequenceEqual(actual)) throw new AssertionException(message);
        }
    }

    /// <summary>
    /// Contains assertions for single values
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Throws an AssertionException if the given values are not equivalent
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The exception message to use if the values are not equal</param>
        public static void AreEqual(object expected, object actual, string message = "")
        {
            if (message == "") message = $"Expected '{expected}' but got '{actual}'";
            if (!expected.Equals(actual)) throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an AssertionException if the given values are not equivalent
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The exception message to use if the values are not equal</param>
        public static void AreApproximatelyEqual(double expected, double actual, double tolerance, string message = "")
        {
            if (message == "") message = $"Expected {expected}±{tolerance} but got '{actual}'";
            if (Math.Abs(expected - actual) > tolerance) throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an AssertionException if the given expression is false
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <param name="message">The exception message to use if the expression is false</param>
        public static void IsTrue(bool expression, string message = "Expected true but got false")
        {
            if (!expression) throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an AssertionException if the given expression is true
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <param name="message">The exception message to use if the expression is true</param>
        public static void IsFalse(bool expression, string message = "Expected false but got true")
        {
            if (expression) throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an AssertionException
        /// </summary>
        /// <param name="message">The exception message to use</param>
        public static void Fail(string message = "")
        {
            throw new AssertionException(message);
        }
    }

    /// <summary>
    /// Enables the running of unit tests
    /// </summary>
    public static class TestRunner
    {
        private const string TICK = "✔";
        private const string CROSS = "✘";

        /// <summary>
        /// Runs the given set of unit tests and outputs results
        /// </summary>
        /// <param name="name">The name of the test category</param>
        /// <param name="tests">The unit tests to run</param>
        public static int RunTests(string name, List<Action> tests)
        {
            Console.WriteLine($"=== Category: {name} ===");
            int passCount = 0;
            StackTrace st = new StackTrace();


            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"\tTest {i + 1} of {tests.Count}: {tests[i].Method.Name}");
                try
                {
                    tests[i]();
                }
                catch (Exception e)
                {
                    if (e is AssertionException)
                    {
                        Console.WriteLine($"\t\t{CROSS} {e.Message}");
                        continue;
                    }
                    var frame = new StackTrace(e, true).GetFrame(st.FrameCount);
                    Console.WriteLine($"\t\t{CROSS} {e.GetType().Name}: {e.Message}");
                    Console.WriteLine($"\t\t  Method: {frame.GetMethod().Name}");
                    Console.WriteLine($"\t\t    File: {System.IO.Path.GetFileName(frame.GetFileName())}");
                    Console.WriteLine($"\t\t    Line: {frame.GetFileLineNumber()}");
                    continue;
                }

                Console.WriteLine($"\t\t{TICK} Test passed");
                passCount++;
            }

            Console.WriteLine($"\t==> {passCount}/{tests.Count} <==\n");
            return passCount;
        }

        /// <summary>
        /// Runs the given set of named unit tests and outputs results
        /// </summary>
        /// <param name="name">The name of the test category</param>
        /// <param name="tests">The named unit tests to run</param>
        public static int RunTests(string name, List<(string, Action)> tests)
        {
            Console.WriteLine($"=== Category: {name} ===");
            int passCount = 0;
            StackTrace st = new StackTrace();

            for (int i = 0; i < tests.Count; i++)
            {
                Console.WriteLine($"\tTest {i + 1} of {tests.Count}: {tests[i].Item1}");
                try
                {
                    tests[i].Item2();
                }
                catch (Exception e)
                {
                    if (e is AssertionException)
                    {
                        Console.WriteLine($"\t\t{CROSS} {e.Message}");
                        continue;
                    }
                    Console.WriteLine($"\t\t{CROSS} {e.GetType().Name}: {e.Message}");
                    var eStackTrace = new StackTrace(e, true);
                    var frame = eStackTrace.GetFrame(st.FrameCount);
                    if (frame != null)
                    {
                        Console.WriteLine($"\t\t  Method: {frame.GetMethod().Name}");
                        Console.WriteLine($"\t\t    File: {System.IO.Path.GetFileName(frame.GetFileName())} @ {frame.GetFileLineNumber()}:{frame.GetFileColumnNumber()}");
                        string line = System.IO.File.ReadAllLines(frame.GetFileName())[frame.GetFileLineNumber() - 1];
                        Console.WriteLine($"\t\t    Code: {line.Trim()}");
                    }
                    continue;
                }

                Console.WriteLine($"\t\t{TICK} Test passed");
                passCount++;
            }

            Console.WriteLine($"\t==> {passCount}/{tests.Count} <==\n");
            return passCount;
        }
    }
}
