using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Add1_TestData))]
    public void Add1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(Add2_TestData))]
    public void Add2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Add1_TestData()
    {
        // N - Number of initial items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Add(1),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 1 },
                    0)
            ],
            ExpectedItems: [1],
            CountChanged: true))
        {
            TestName = "N == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Add(2),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 2 },
                    1)
            ],
            ExpectedItems: [1, 2],
            CountChanged: true))
        {
            TestName = "N == 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Add(4),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 4 },
                    3)
            ],
            ExpectedItems: [1, 2, 3, 4],
            CountChanged: true))
        {
            TestName = "N > 1"
        };
    }

    private static IEnumerable Add2_TestData()
    {
        // T - Type of the added item.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Add((object)1),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    new[] { 1 },
                    0)
            ],
            ExpectedItems: [1],
            CountChanged: true))
        {
            TestName = "T == int"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Add(new object()),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "T == object"
        };
    }
}