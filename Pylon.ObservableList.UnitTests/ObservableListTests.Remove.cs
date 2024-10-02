using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Remove1_TestData))]
    public void Remove1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(Remove2_TestData))]
    public void Remove2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Remove1_TestData()
    {
        // Q - Item exists in list.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Remove(0),
            ExpectedEvents: [],
            ExpectedItems: [1, 2, 3],
            CountChanged: false))
        {
            TestName = "Q == false"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Remove(2),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new[] { 2 },
                    1)
            ],
            ExpectedItems: [1, 3],
            CountChanged: true))
        {
            TestName = "Q == true"
        };
    }

    private static IEnumerable Remove2_TestData()
    {
        // T - Type of the added item.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Remove((object)1),
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
            TestName = "T == int"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Remove(new object()),
            ExpectedEvents: [],
            ExpectedItems: [1],
            CountChanged: false))
        {
            TestName = "T == object"
        };
    }
}