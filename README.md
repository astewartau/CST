# CST: C# Unit Testing Framework

CST is a super simple C# unit testing framework. Add the `CST_Test.cs` file to your project, and a `using CST;` at the top of any file you wish to use it in.

## Demonstration

A test `Program.cs` file has been provided that demonstrates some of the basic functionality against a `Divide` method. The code is pasted below:

```c#
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
                ("Positive numbers", Test_Divide_1_Positive_Numbers),
                ("Negative numbers", Test_Divide_2_Negative_Numbers)
                ("Divide by zero", Test_Divide_3_Denominator_Zero_Exception),
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
```

```
=== Category: Divide ===
	Test 1 of 3: Positive numbers
		✔ Test passed
	Test 2 of 3: Negative numbers
		✔ Test passed
	Test 3 of 3: Divide by zero
		✔ Test passed
	==> 3/3 <==
```

## Using CST

First, write unit tests in the form of methods. Unit tests should use the `Assert` or `CollectionAssert` classes to make assertions. For example:

```c#
// basic unit test
static void Test_Add_OnePlusOne()
{
    Assert.AreEqual(
        expected: 2,
        actual: Add(1, 1)
    );
}

// unit test with custom message
static void Test_Add_OneMinusOne()
{
    int expected = 0;
    int actual = Add(1, -1);

    Assert.AreEqual(
        expected: expected,
        actual: actual,
        message: $"Expected '{expected}' but Add(1, -1) returned '{actual}'"
    );
}
```

Then, run the tests using the `TestRunner` like so:

```c#
TestRunner.RunTests(
    name: "Add",
    tests: new List<(string, Action)>()
    {
        ("One plus one", Test_Add_OnePlusOne),
        ("One minus one", Test_Add_OneMinusOne)
    }
);
```

The output of the program is as follows:

```
=== Category: Add ===
	Test 1 of 2: One plus one
		✔ Test passed
	Test 2 of 2: One minus one
		✔ Test passed
	==> 2/2 <==
```

With an invalid `Add` method that returns the value `5`, the output is:

```
=== Category: Add ===
	Test 1 of 2: One plus one
		✔ Expected '2' but got '5'
	Test 2 of 2: One minus one
		✔ Expected '0' but Add(1, -1) returned '5'
	==> 2/2 <==
```

## Assertion types

There are a range of assertions available, including:

- `Assert.AreEqual(...)`
- `Assert.AreApproximatelyEqual(...)`
- `Assert.IsTrue(...)`
- `Assert.IsFalse(...)`
- `Assert.Fail(...)`
- `CollectionAssert.AreEqual(...)`

## Contributions

I am happy to accept pull requests if I feel that they add value without a substantial complexity cost.
