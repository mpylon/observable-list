using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Pylon.ObservableList.UnitTests;

[TestFixture]
public sealed partial class ObservableListTests
{
    [Test]
    public void CopyTo1()
    {
        // Arrange

        ObservableList<int> source = [1, 2, 3, 4, 5];
        var target = Array.CreateInstance(typeof(object), 6);

        // Act

        source.CopyTo(target, 1);

        // Assert

        target
            .Should()
            .BeEquivalentTo(new object?[] { null, 1, 2, 3, 4, 5 }, o => o.WithStrictOrdering());
    }

    [Test]
    public void CopyTo2()
    {
        // Arrange

        ObservableList<int> source = [1, 2, 3, 4, 5];
        var target = Array.CreateInstance(typeof(object), 5, 5);

        // Act

        var action = () =>
        {
            source.CopyTo(target, 0);
        };

        // Assert

        action
            .Should()
            .Throw<ArgumentException>();
    }

    [Test]
    public void CopyTo3()
    {
        // Arrange

        ObservableList<int> source = [1, 2, 3, 4, 5];
        var target = Array.CreateInstance(typeof(object), 5);

        // Act

        var action = () =>
        {
            source.CopyTo(target, 1);
        };

        // Assert

        action
            .Should()
            .Throw<ArgumentException>();
    }

    /// <summary>
    /// Test data for verifying the behavior of <see cref="ObservableList{T}"/>.
    /// </summary>
    public interface IMutableOperationTestData
    {
        void Assert();
    }

    /// <summary>
    /// Test data for verifying the behavior of <see cref="ObservableList{T}"/> in an exceptional scenario.
    /// </summary>
    /// <param name="ActualItems">The list being tested.</param>
    /// <param name="Action">The action to be tested.</param>
    /// <typeparam name="TException">The type of the expected exception.</typeparam>
    public sealed record InvalidMutableOperationTestData<TException>(
        ObservableList<int> ActualItems,
        Action<ObservableList<int>> Action)
        : IMutableOperationTestData
        where TException : Exception
    {
        /// <inheritdoc />
        public void Assert()
        {
            var action = () => Action(ActualItems);

            action
                .Should()
                .Throw<TException>();
        }
    }

    /// <summary>
    /// Test data for verifying the behavior of <see cref="ObservableList{T}" /> in a non-exceptional scenario.
    /// </summary>
    /// <param name="ActualItems">The list being tested.</param>
    /// <param name="Action">The action to be tested.</param>
    /// <param name="ExpectedEvents">The expected <see cref="INotifyCollectionChanged" /> events.</param>
    /// <param name="ExpectedItems">The expected items in the list.</param>
    public sealed record ValidMutableOperationTestData(
        ObservableList<int> ActualItems,
        Action<ObservableList<int>> Action,
        NotifyCollectionChangedEventArgs[] ExpectedEvents,
        int[] ExpectedItems,
        bool CountChanged)
        : IMutableOperationTestData
    {
        /// <inheritdoc />
        public void Assert()
        {
            // Arrange

            var countChanged = false;
            var eventIndex = 0;

            var itemsControl = new ItemsControl
            {
                ItemsPanel = new FuncTemplate<Panel?>(() => new Panel()),
                ItemsSource = ActualItems
            };

            var window = new Window
            {
                Content = itemsControl,
                Height = 768,
                Width = 1024
            };

            window.Show();

            using (new AssertionScope())
            {
                ActualItems.CollectionChanged += OnCollectionChanged;
                ActualItems.PropertyChanged += OnPropertyChanged;

                try
                {
                    // Act

                    Action(ActualItems);
                }
                finally
                {
                    ActualItems.CollectionChanged -= OnCollectionChanged;
                    ActualItems.PropertyChanged -= OnPropertyChanged;
                    window.Close();
                }

                // Assert

                var actualContents = itemsControl
                    .ItemsPanelRoot!
                    .GetVisualChildren()
                    .Cast<ContentPresenter>()
                    .Select(x => x.Content);

                actualContents
                    .Should()
                    .BeEquivalentTo(ActualItems, o => o.WithStrictOrdering());

                ActualItems
                    .Should()
                    .BeEquivalentTo(ExpectedItems, o => o.WithStrictOrdering());

                eventIndex
                    .Should()
                    .Be(ExpectedEvents.Length);

                countChanged
                    .Should()
                    .Be(CountChanged);
            }

            return;

            void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs actualEvent)
            {
                // Assert

                eventIndex
                    .Should()
                    .BeLessThan(ExpectedEvents.Length);

                actualEvent
                    .Should()
                    .BeEquivalentTo(ExpectedEvents[eventIndex++], o => o.WithStrictOrdering());
            }

            void OnPropertyChanged(object? sender, PropertyChangedEventArgs actualEvent)
            {
                // Assert

                if (actualEvent.PropertyName == nameof(ObservableList<int>.Count))
                {
                    countChanged = true;
                }
            }
        }
    }
}