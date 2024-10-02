using System.Collections.Generic;
using System.Collections.Specialized;

namespace Pylon.ObservableList;

public static class ObservableListExtensions
{
    /// <inheritdoc cref="List{T}.AddRange" />
    public static void AddRange<T>(this ObservableList<T> list, IEnumerable<T> collection)
    {
        var index = list.Items.Count;
        list.Items.AddRange(collection);

        if (list.Items.Count > index)
        {
            if (list.CanNotify)
            {
                var @event = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    list.Items[index..],
                    index);

                list.NotifyCollectionChanged(@event);
            }

            list.NotifyCountChanged();
        }
    }

    /// <inheritdoc cref="List{T}.InsertRange" />
    public static void InsertRange<T>(this ObservableList<T> list, int index, IEnumerable<T> collection)
    {
        var oldItemsCount = list.Items.Count;
        list.Items.InsertRange(index, collection);
        var collectionCount = list.Items.Count - oldItemsCount;

        if (collectionCount > 0)
        {
            if (list.CanNotify)
            {
                var @event = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    list.Items[index..(index + collectionCount)],
                    index);

                list.NotifyCollectionChanged(@event);
            }

            list.NotifyCountChanged();
        }
    }

    /// <summary>
    /// Moves the item at <see cref="oldIndex"/> to <see cref="newIndex"/> in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="oldIndex">The old index.</param>
    /// <param name="newIndex">The new index.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void Move<T>(this ObservableList<T> list, int oldIndex, int newIndex)
    {
        if (newIndex > oldIndex)
        {
            list.Items.RotateRight(oldIndex, newIndex - oldIndex + 1, 1);
        }
        else if (newIndex < oldIndex)
        {
            list.Items.RotateLeft(newIndex, oldIndex - newIndex + 1, 1);
        }
        else
        {
            return;
        }

        if (list.CanNotify)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Move,
                new[] { list[newIndex] },
                newIndex,
                oldIndex);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <summary>
    /// Moves the <see cref="count"/> items starting at <see cref="oldIndex"/> to <see cref="newIndex"/> in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="oldIndex">The old index.</param>
    /// <param name="count">The number of items to move.</param>
    /// <param name="newIndex">The new index.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void MoveRange<T>(this ObservableList<T> list, int oldIndex, int count, int newIndex)
    {
        var changedItems = list.Items[oldIndex..(oldIndex + count)];

        if (newIndex > oldIndex)
        {
            list.Items.RotateLeft(oldIndex, newIndex - oldIndex + 1, count);
        }
        else if (newIndex < oldIndex)
        {
            list.Items.RotateRight(newIndex, oldIndex - newIndex + count, count);
        }
        else
        {
            return;
        }

        if (list.CanNotify)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Move,
                changedItems,
                newIndex,
                oldIndex);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <summary>
    /// Removes <see cref="items"/> from <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="items">The items.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void RemoveAll<T>(this ObservableList<T> list, IEnumerable<T> items)
    {
        var set = new HashSet<T>(items);
        var count = 0;

        for (var i = list.Items.Count - 1; i >= 0; --i)
        {
            if (set.Contains(list.Items[i]))
            {
                ++count;
            }
            else if (count > 0)
            {
                list.RemoveRange(i + 1, count);
                count = 0;
            }
        }

        if (count > 0)
        {
            list.RemoveRange(0, count);
        }
    }

    /// <inheritdoc cref="List{T}.RemoveRange" />
    public static void RemoveRange<T>(this ObservableList<T> list, int index, int count)
    {
        var changedItems = list.Items[index..(index + count)];
        list.Items.RemoveRange(index, count);

        if (count > 0)
        {
            if (list.CanNotify)
            {
                var @event = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    changedItems,
                    index);

                list.NotifyCollectionChanged(@event);
            }

            list.NotifyCountChanged();
        }
    }

    /// <inheritdoc cref="List{T}.Reverse()" />
    public static void Reverse<T>(this ObservableList<T> list)
    {
        var oldItems = list.Items[..];
        list.Items.Reverse();

        if (list.CanNotify && list.Items.Count > 1)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                list.Items,
                oldItems,
                0);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <inheritdoc cref="List{T}.Reverse(int, int)" />
    public static void Reverse<T>(this ObservableList<T> list, int index, int count)
    {
        var oldItems = list.Items[index..(index + count)];
        list.Items.Reverse(index, count);
        var newItems = list.Items[index..(index + count)];

        if (list.CanNotify && count > 1)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                newItems,
                oldItems,
                index);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <summary>
    /// Rotates all items left by <see cref="count"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="count">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void RotateLeft<T>(this ObservableList<T> list, int count)
    {
        if (list.Items.Count < 2)
        {
            return;
        }

        var oldItems = list.Items[..];
        count = MathUtilities.Mod(count, list.Items.Count);
        list.Items.Reverse(0, count);
        list.Items.Reverse(count, MathUtilities.Mod(list.Items.Count - count, list.Items.Count));
        list.Items.Reverse();

        if (list.CanNotify)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                list.Items,
                oldItems,
                0);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <summary>
    /// Rotates the <see cref="itemCount"/> items starting at <see cref="index"/> left by <see cref="rotateCount"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="index">The starting index.</param>
    /// <param name="itemCount">The number of items.</param>
    /// <param name="rotateCount">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void RotateLeft<T>(this ObservableList<T> list, int index, int itemCount, int rotateCount)
    {
        var oldItems = list.Items[index..(index + itemCount)];
        var rotated = list.Items.RotateLeft(index, itemCount, rotateCount);

        if (list.CanNotify && rotated)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                list.Items[index..(index + itemCount)],
                oldItems,
                index);

            list.NotifyCollectionChanged(@event);
        }
    }

    /// <summary>
    /// Rotates all items right by <see cref="count"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="count">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void RotateRight<T>(this ObservableList<T> list, int count)
    {
        list.RotateLeft(-count);
    }

    /// <summary>
    /// Rotates the <see cref="itemCount"/> items starting at <see cref="index"/> right by <see cref="rotateCount"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="index">The starting index.</param>
    /// <param name="itemCount">The number of items.</param>
    /// <param name="rotateCount">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public static void RotateRight<T>(this ObservableList<T> list, int index, int itemCount, int rotateCount)
    {
        list.RotateLeft(index, itemCount, -rotateCount);
    }
}