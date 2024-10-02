using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pylon.ObservableList;

internal static class ArgumentExceptionUtilities
{
    /// <summary>
    /// Throws an exception if the range defined by <see cref="index"/> and <see cref="count"/> extends beyond <see cref="collection"/>.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="index">The starting index of the range.</param>
    /// <param name="count">The number of items in the range.</param>
    /// <param name="collectionParamName">The name of the collection parameter.</param>
    /// <param name="indexParamName">The name of the index parameter.</param>
    /// <param name="countParamName">The name of the count parameter.</param>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <exception cref="ArgumentException">
    /// Range defined by <see cref="index"/> and <see cref="count"/> extends beyond <see cref="collection"/>.
    /// </exception>
    public static void ThrowIfRangeOutOfBounds<T, TCollection>(
        TCollection collection,
        int index,
        int count,
        [CallerArgumentExpression(nameof(collection))]
        string? collectionParamName = default,
        [CallerArgumentExpression(nameof(index))]
        string? indexParamName = default,
        [CallerArgumentExpression(nameof(count))]
        string? countParamName = default)
        where TCollection : ICollection<T>
    {
        if (index + count > collection.Count)
        {
            throw new ArgumentException($"Range defined by {indexParamName} and {countParamName} extends beyond {collectionParamName}.");
        }
    }
}