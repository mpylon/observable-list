using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(RemoveAt_TestData))]
    public void RemoveAt(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable RemoveAt_TestData()
    {
        // N - Number of initial items.
        // I - Index of removed item.

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [1],
            Action: x => x.RemoveAt(-1)))
        {
            TestName = "N == 1, I < 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.RemoveAt(0),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1 },
                    0)
            ],
            ExpectedItems: [],
            CountChanged: true))
        {
            TestName = "N == 1, I == 0"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [1],
            Action: x => x.RemoveAt(1)))
        {
            TestName = "N == 1, I == N"
        };
    }
}