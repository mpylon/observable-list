using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(AddRange_TestData))]
    public void AddRange(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable AddRange_TestData()
    {
        // N - Number of initial items.
        // C - Number of items to add.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.AddRange([]),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, C == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.AddRange([4, 5, 6]),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 4, 5, 6 },
                    3)
            ],
            ExpectedItems: [1, 2, 3, 4, 5, 6],
            CountChanged: true))
        {
            TestName = "N > 1, C > 1"
        };
    }
}