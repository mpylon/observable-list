using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(MoveRange_TestData))]
    public void MoveRange(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable MoveRange_TestData()
    {
        // N - Number of initial items.
        // OI - Old index of items.
        // C - Number of items to move.
        // NI - New index of items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.MoveRange(0, 0, 0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, OI == 0, C == 0, NI == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.MoveRange(0, 2, 3),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    new[] { 1, 2 },
                    3,
                    0)
            ],
            ExpectedItems: [3, 4, 1, 2, 5],
            CountChanged: false))
        {
            TestName = "N == 5, OI == 0, C == 2, N - 1 > NI > OI"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.MoveRange(0, 2, 4),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    new[] { 1, 2 },
                    4,
                    0)
            ],
            ExpectedItems: [3, 4, 5, 1, 2],
            CountChanged: false))
        {
            TestName = "N == 5, OI == 0, C == 2, NI == N - 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.MoveRange(1, 2, 1),
            ExpectedEvents: [],
            ExpectedItems: [1, 2, 3, 4, 5],
            CountChanged: false))
        {
            TestName = "N == 5, OI > 0, C == 2, NI == OI"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.MoveRange(3, 2, 0),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    new[] { 4, 5 },
                    0,
                    3)
            ],
            ExpectedItems: [4, 5, 1, 2, 3],
            CountChanged: false))
        {
            TestName = "N == 5, OI == N - C - 1, C == 2, NI == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.MoveRange(4, 1, 0),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    new[] { 5 },
                    0,
                    4)
            ],
            ExpectedItems: [5, 1, 2, 3, 4],
            CountChanged: false))
        {
            TestName = "N == 5, OI == N - C - 1, C == 1, NI == 0"
        };
    }
}