using System;
using System.Collections.Generic;
using static Pylon.ObservableList.MathUtilities;

namespace Pylon.ObservableList;

public static class ListExtensions
{
    /// <summary>
    /// Rotates the <see cref="itemCount"/> items starting at <see cref="index"/> left by <see cref="rotateCount"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="index">The starting index.</param>
    /// <param name="itemCount">The number of items.</param>
    /// <param name="rotateCount">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <returns>Whether the list was mutated or not.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <see cref="index"/> or <see cref="itemCount"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="index"/> and <see cref="itemCount"/> specify an invalid range in <see cref="list"/>.
    /// </exception>
    public static bool RotateLeft<T>(this List<T> list, int index, int itemCount, int rotateCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(itemCount, nameof(itemCount));
        ArgumentExceptionUtilities.ThrowIfRangeOutOfBounds<T, List<T>>(list, index, itemCount);

        if (itemCount < 2)
        {
            return false;
        }

        rotateCount = Mod(rotateCount, itemCount);

        if (rotateCount == 0)
        {
            return false;
        }

        list.Reverse(index, rotateCount);
        list.Reverse(index + rotateCount, Mod(itemCount - rotateCount, itemCount));
        list.Reverse(index, itemCount);
        return true;
    }

    /// <summary>
    /// Rotates the <see cref="itemCount"/> items starting at <see cref="index"/> right by <see cref="rotateCount"/> places in <see cref="list"/>.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="index">The starting index.</param>
    /// <param name="itemCount">The number of items.</param>
    /// <param name="rotateCount">The number of places.</param>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <returns>Whether the list was mutated or not.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <see cref="index"/> or <see cref="itemCount"/> is negative.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="index"/> and <see cref="itemCount"/> specify an invalid range in <see cref="list"/>.
    /// </exception>
    public static bool RotateRight<T>(this List<T> list, int index, int itemCount, int rotateCount)
    {
        return list.RotateLeft(index, itemCount, -rotateCount);
    }
}