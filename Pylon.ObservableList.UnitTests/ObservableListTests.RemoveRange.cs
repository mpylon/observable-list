using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(RemoveRange_TestData))]
    public void RemoveRange(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable RemoveRange_TestData()
    {
        // N - Number of initial items.
        // I - Index of removed item.
        // C - Number of removed items.

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [],
            Action: x => x.RemoveRange(-1, 0)))
        {
            TestName = "N == 0, I < 0, C == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RemoveRange(0, 0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, I == 0, C == 0"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentException>(
            ActualItems: [],
            Action: x => x.RemoveRange(1, 0)))
        {
            TestName = "N == 0, I > N, C == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.RemoveRange(0, 2),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1, 2 },
                    0)
            ],
            ExpectedItems: [3],
            CountChanged: true))
        {
            TestName = "N > 1, I == 0, C > 1"
        };
    }
}