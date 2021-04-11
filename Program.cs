using System;
using System.Collections.Generic;
using CST;

class Program
{
    // ====================================
    // ===     METHOD UNDER TESTING     ===
    // ====================================

    static double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException($"Cannot divide {a} by zero!");
        }

        return a / b;
    }

    // ====================================
    // === MAIN METHOD - EXECUTES TESTS ===
    // ====================================`
    static void Main(string[] args)
    {
        TestRunner.RunTests(
            name: "Divide",
            tests: new List<(string, Action)>()
            {
                ("Divide by zero", Test_Divide_3_Denominator_Zero_Exception),
                ("Positive numbers", Test_Divide_1_Positive_Numbers),
                ("Negative numbers", Test_Divide_2_Negative_Numbers)
            }
        );
    }

    // ====================================
    // ===         TEST METHODS         ===
    // ====================================

    static void Test_Divide_1_Positive_Numbers()
    {
        Random rng = new Random();
        for (int i = 0; i < 100; i++)
        {
            double numerator = rng.NextDouble() * 1000;
            double denominator = rng.NextDouble() * 1000;
            double expected = numerator / denominator;
            double actual = Divide(numerator, denominator);

            Assert.AreEqual(
                expected: expected,
                actual: actual,
                message: $"Expected {expected} for Divide({numerator},{denominator}), but got {actual}"
            );
        }
    }

    static void Test_Divide_2_Negative_Numbers()
    {
        Random rng = new Random();
        for (int i = 0; i < 100; i++)
        {
            double numerator = rng.NextDouble() * 1000 - 1000;
            double denominator = rng.NextDouble() * 1000 - 1000;

            Assert.AreEqual(
                expected: numerator / denominator,
                actual: Divide(numerator, denominator)
            );
        }
    }

    static void Test_Divide_3_Denominator_Zero_Exception()
    {
        double result;
        try
        {
            result = Divide(1, 0);
            Assert.Fail($"Expected System.DivideByZeroException but got return value: {result}");
        }
        catch (DivideByZeroException e)
        {
            Assert.AreEqual(
                expected: "Cannot divide 1 by zero!",
                actual: e.Message
            );
        }
        catch (AssertionException e)
        {
            throw e;
        }
        catch (Exception e)
        {
            Assert.Fail($"Expected System.DivideByZeroException but got {e.GetType()}");
        }
    }
}
