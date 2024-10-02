using System.Collections;
using System.Collections.Specialized;
using Avalonia.Headless.NUnit;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

public sealed partial class ObservableListTests
{
    [AvaloniaTest]
    [TestCaseSource(nameof(RotateRight1_TestData))]
    public void RotateRight1(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    [AvaloniaTest]
    [TestCaseSource(nameof(RotateRight2_TestData))]
    public void RotateRight2(IMutableOperationTestData data)
    {
        // Act & Assert

        data.Assert();
    }

    private static IEnumerable RotateRight1_TestData()
    {
        // N - Number of initial items.
        // C - Number of places to rotate.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RotateRight(-1),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, C < 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RotateRight(0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, C == 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RotateRight(1),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, C > 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1],
            Action: x => x.RotateRight(1),
            ExpectedEvents: [],
            ExpectedItems: [1],
            CountChanged: false))
        {
            TestName = "N == 1, C > 0"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3],
            Action: x => x.RotateRight(2),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 2, 3, 1 },
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [2, 3, 1],
            CountChanged: false))
        {
            TestName = "N > 1, C > 0"
        };
    }

    private static IEnumerable RotateRight2_TestData()
    {
        // N - Number of initial items.
        // C - Number of places to rotate.
        // R - Range of items to rotate.

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [],
            Action: x => x.RotateRight(0, 0, 0),
            ExpectedEvents: [],
            ExpectedItems: [],
            CountChanged: false))
        {
            TestName = "N == 0, C == 0, R = [0, 0)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.RotateRight(1, 3, 3),
            ExpectedEvents: [],
            ExpectedItems: [1, 2, 3, 4, 5],
            CountChanged: false))
        {
            TestName = "N > 1, C == N, R = [1, 4)"
        };

        yield return new TestCaseData(new ValidMutableOperationTestData(
            ActualItems: [1, 2, 3, 4, 5],
            Action: x => x.RotateRight(0, 3, 2),
            ExpectedEvents:
            [
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new[] { 2, 3, 1 },
                    new[] { 1, 2, 3 },
                    0)
            ],
            ExpectedItems: [2, 3, 1, 4, 5],
            CountChanged: false))
        {
            TestName = "N > 1, N > C > 0, R == [0, 3)"
        };
    }
}