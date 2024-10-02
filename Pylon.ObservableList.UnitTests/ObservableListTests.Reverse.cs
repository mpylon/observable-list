using System;
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(Reverse1_TestData))]
    public void Reverse1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(Reverse2_TestData))]
    public void Reverse2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable Reverse1_TestData()
    {
        // N - Number of initial items.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Reverse(),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Reverse(),
            ExpectedEvents: [],
            ExpectedItems: [1],
            CountChanged: false))
        {
            TestName = "N == 1"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.Reverse(),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 3, 2, 1 },
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [3, 2, 1],
            CountChanged: false))
        {
            TestName = "N > 1"
        };
    }

    private static IEnumerable Reverse2_TestData()
    {
        // N - Number of initial items.
        // R - Range to be reversed.

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [],
            Action: x => x.Reverse(-1, 0)))
        {
            TestName = "N == 0, R == [-1, -1)"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [],
            Action: x => x.Reverse(-1, 1)))
        {
            TestName = "N == 0, R == [-1, 0)"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentOutOfRangeException>(
            ActualItems: [],
            Action: x => x.Reverse(-1, 2)))
        {
            TestName = "N == 0, R == [-1, 1)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.Reverse(0, 0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, R == [0, 0)"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentException>(
            ActualItems: [],
            Action: x => x.Reverse(0, 1)))
        {
            TestName = "N == 0, R == [0, 1)"
        };

        yield return new TestCaseData(new InvalidMutableOperationTestData<ArgumentException>(
            ActualItems: [],
            Action: x => x.Reverse(1, 0)))
        {
            TestName = "N == 0, R == [1, 1)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.Reverse(0, 1),
            ExpectedEvents: [],
            ExpectedItems: [1],
            CountChanged: false))
        {
            TestName = "N == 1, R == [0, 1)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.Reverse(0, 3),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 3, 2, 1 },
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [3, 2, 1, 4, 5],
            CountChanged: false))
        {
            TestName = "N > 1, R == [0, 3)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.Reverse(1, 3),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 4, 3, 2 },
                    new[] { 2, 3, 4 },
                    1)
            ],
            ExpectedItems: [1, 4, 3, 2, 5],
            CountChanged: false))
        {
            TestName = "N > 1, R == [1, 4)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.Reverse(2, 3),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 5, 4, 3 },
                    new[] { 3, 4, 5 },
                    2)
            ],
            ExpectedItems: [1, 2, 5, 4, 3],
            CountChanged: false))
        {
            TestName = "N > 1, R == [2, 5)"
        };
    }
}