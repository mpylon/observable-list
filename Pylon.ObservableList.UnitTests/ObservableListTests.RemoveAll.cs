using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(RemoveAll_TestData))]
    public void RemoveAll(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable RemoveAll_TestData()
    {
        // N - Number of initial items.
        // R - Ranges of items to be removed.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RemoveAll([]),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, R == empty"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.RemoveAll([1, 2]),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1, 2 },
                    0)
            ],
            ExpectedItems: [3, 4, 5],
            CountChanged: true))
        {
            TestName = "N > 1, R == [0, 2)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.RemoveAll([1, 2, 4, 5]),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 4, 5 },
                    3),
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1, 2 },
                    0)
            ],
            ExpectedItems: [3],
            CountChanged: true))
        {
            TestName = "N > 1, R == [0, 2) || [3, 5)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5, 6, 7],
            Action: x => x.RemoveAll([1, 2, 4, 5, 7]),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 7 },
                    6),
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 4, 5 },
                    3),
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1, 2 },
                    0)
            ],
            ExpectedItems: [3, 6],
            CountChanged: true))
        {
            TestName = "N > 1, R == [0, 2) || [3, 5) || [6, 7)"
        };
    }
}