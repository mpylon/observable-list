using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Pylon.ObservableList;

/// <summary>
/// An <see cref="IList{T}"/> that emits <see cref="INotifyCollectionChanged"/> events on mutation.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public sealed class ObservableList<T> : IList, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    /// <summary>
    /// Whether listeners have been added to <see cref="CollectionChanged"/> or not.
    /// </summary>
    public bool CanNotify => CollectionChanged != null;

    public int Count => Items.Count;
    public bool IsFixedSize => false;
    public bool IsReadOnly => false;
    public bool IsSynchronized => false;

    /// <summary>
    /// The underlying list of items.
    /// </summary>
    public List<T> Items { get; } = [];

    public object SyncRoot => this;

    public void Add(T item)
    {
        var index = Items.Count;
        Items.Add(item);

        if (CollectionChanged != null)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                item,
                index);

            CollectionChanged.Invoke(this, @event);
        }

        NotifyCountChanged();
    }

    public int Add(object? value)
    {
        if (value is not T tValue)
        {
            return -1;
        }

        Add(tValue);
        return Count;
    }

    public void Clear()
    {
        var oldItems = Items[..];
        Items.Clear();

        if (oldItems.Count > 0)
        {
            if (CollectionChanged != null)
            {
                var @event = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    oldItems,
                    0);

                NotifyCollectionChanged(@event);
            }

            NotifyCountChanged();
        }
    }

    public bool Contains(object? value)
    {
        return value is T tValue && Contains(tValue);
    }

    public bool Contains(T item)
    {
        return Items.Contains(item);
    }

    public void CopyTo(Array array, int index)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        if (array.Rank != 1)
        {
            throw new ArgumentException("Multi-dimensional arrays are not supported.");
        }

        if (array.Length - index < Count)
        {
            throw new ArgumentException("The target array is too small.");
        }

        if (array is T[] tArray)
        {
            Items.CopyTo(tArray, index);
        }
        else
        {
            for (int i = 0, j = index; i < Items.Count; ++i, ++j)
            {
                array.SetValue(Items[i], j);
            }
        }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    public int IndexOf(object? value)
    {
        return value is T tValue ? IndexOf(tValue) : -1;
    }

    public int IndexOf(T item)
    {
        return Items.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        Items.Insert(index, item);

        if (CollectionChanged != null)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                item,
                index);

            CollectionChanged.Invoke(this, @event);
        }

        NotifyCountChanged();
    }

    public void Insert(int index, object? value)
    {
        Insert(index, (T)value!);
    }

    /// <summary>
    /// Notify listeners of <see cref="CollectionChanged"/> that <see cref="@event"/> has occurred.
    /// </summary>
    /// <param name="event">The event that occurred.</param>
    public void NotifyCollectionChanged(NotifyCollectionChangedEventArgs @event)
    {
        CollectionChanged?.Invoke(this, @event);
    }

    /// <summary>
    /// Notify listeners of <see cref="PropertyChanged"/> that <see cref="Count"/> has changed.
    /// </summary>
    public void NotifyCountChanged()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);

        if (index < 0)
        {
            return false;
        }

        RemoveAt(index);
        return true;
    }

    public void Remove(object? value)
    {
        if (value is T tValue)
        {
            Remove(tValue);
        }
    }

    public void RemoveAt(int index)
    {
        var changedItem = Items[index];
        Items.RemoveAt(index);

        if (CollectionChanged != null)
        {
            var @event = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Remove,
                changedItem,
                index);

            CollectionChanged.Invoke(this, @event);
        }

        NotifyCountChanged();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public T this[int index]
    {
        get => Items[index];
        set
        {
            var oldItem = Items[index];
            Items[index] = value;

            if (CollectionChanged != null)
            {
                var @event = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    value,
                    oldItem,
                    index);

                CollectionChanged.Invoke(this, @event);
            }
        }
    }

    object? IList.this[int index]
    {
        get => this[index];
        set => this[index] = (T)value!;
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
}