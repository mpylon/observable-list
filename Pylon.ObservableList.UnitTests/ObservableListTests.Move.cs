using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Move_TestData))]
    public void Move(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Move_TestData()
    {
        // N - Number of initial items.
        // OI - Old index of item.
        // NI - New index of item.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Move(0, 0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, OI == 0, NI == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Move(0, 0),
            ExpectedEvents: [],
            ExpectedItems: [1],
            CountChanged: false))
        {
            TestName = "N == 1, OI == 0, NI == 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Move(0, 1),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    new[] { 1 },
                    1,
                    0)
            ],
            ExpectedItems: [2, 1, 3],
            CountChanged: false))
        {
            TestName = "N == 1, OI == 0, N > NI > 0"
        };
    }
}