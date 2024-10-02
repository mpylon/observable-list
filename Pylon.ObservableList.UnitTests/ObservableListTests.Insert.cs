using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Insert1_TestData))]
    public void Insert1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(Insert2_TestData))]
    public void Insert2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Insert1_TestData()
    {
        // N - Number of initial items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Insert(0, 1),
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
    }

    private static IEnumerable Insert2_TestData()
    {
        // T - Type of the added item.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Insert(0, (object)1),
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

        yield return new TestCaseData(new InvalidMutableOperationTestData<InvalidCastException>(
            ActualItems: [],
            Action: x => x.Insert(0, new object())))
        {
            TestName = "T == object"
        };
    }
}