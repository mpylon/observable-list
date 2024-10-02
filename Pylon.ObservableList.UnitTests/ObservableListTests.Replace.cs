using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Replace1_TestData))]
    public void Replace1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(Replace2_TestData))]
    public void Replace2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Replace1_TestData()
    {
        // N - Number of initial items.
        // I - Index of replaced item.

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [1],
            Action: x => x[-1] = 0))
        {
            TestName = "N == 1, I < 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x[0] = 2,
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 2 },
                    new[] { 1 },
                    0)
            ],
            ExpectedItems: [2],
            CountChanged: false))
        {
            TestName = "N == 1, I == 0"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [1],
            Action: x => x[1] = 0))
        {
            TestName = "N == 1, I == N"
        };
    }

    private static IEnumerable Replace2_TestData()
    {
        // T - Type of the added item.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => ((IList)x)[0] = 2,
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 2 },
                    new[] { 1 },
                    0)
            ],
            ExpectedItems: [2],
            CountChanged: false))
        {
            TestName = "T == int"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<InvalidCastException>(
            ActualItems: [1],
            Action: x => ((IList)x)[0] = new object()))
        {
            TestName = "T == object"
        };
    }
}