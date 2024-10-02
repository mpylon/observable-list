using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Clear_TestData))]
    public void Clear(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Clear_TestData()
    {
        // N - Number of initial items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Clear(),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Clear(),
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
            TestName = "N == 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Clear(),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [],
            CountChanged: true))
        {
            TestName = "N > 1"
        };
    }
}