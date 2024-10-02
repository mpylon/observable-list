using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(InsertRange_TestData))]
    public void InsertRange(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable InsertRange_TestData()
    {
        // N - Number of initial items.
        // I - Index of inserted items.
        // C - Number of inserted items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.InsertRange(0, ArraySegment<int>.Empty),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, I == 0, C == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.InsertRange(0, new[] { 1, 2, 3 }),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [1, 2, 3],
            CountChanged: true))
        {
            TestName = "N == 0, I == 0, C > 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 4, 5],
            Action: x => x.InsertRange(1, new[] { 2, 3 }),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 2, 3 },
                    1)
            ],
            ExpectedItems: [1, 2, 3, 4, 5],
            CountChanged: true))
        {
            TestName = "N > 1, N > I > 0, C > 1"
        };
    }
}