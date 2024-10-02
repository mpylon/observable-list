# Pylon.ObservableList

`ObservableList<T>` is a simple, extensible list that emits `INotifyCollectionChanged` events for each mutable
operation. If you find `ObservableCollection<T>` or `AvaloniaList<T>` incomplete for your use case, consider
using and extending `ObservableList<T>` with your own custom operations.

## Getting Started

Install the `Pylon.ObservableList` package.

```shell
dotnet add package Pylon.ObservableList
```

## Compatibility

`ObservableList<T>` is intended to be used with Avalonia and is compatible with the mutable operations of
`AvaloniaList<T>`. However, it should be usable with any `INotifyCollectionChanged` consumers. Listed below is the
comparison between the mutable operations of each list.


| `ObservableList<T>`                | `AvaloniaList<T>`                  |
| ---------------------------------- | ---------------------------------- |
| `Add(T)`                           | `Add(T)`                           |
| `AddRange(IEnumerable<T>)`         | `AddRange(IEnumerable<T>)`         |
| `Clear()`                          | `Clear()`                          |
| `Insert(int, T)`                   | `Insert(int, T)`                   |
| `InsertRange(int, IEnumerable<T>)` | `InsertRange(int, IEnumerable<T>)` |
| `Move(int int)`                    | `Move(int int)`                    |
| `MoveRange(int, int, int)`         | `MoveRange(int, int, int)`         |
| `Remove(T)`                        | `Remove(T)`                        |
| `RemoveAll(IEnumerable<T>)`        | `RemoveAll(IEnumerable<T>)`        |
| `RemoveAt(int)`                    | `RemoveAt(int)`                    |
| `RemoveRange(int, int)`            | `RemoveRange(int, int)`            |
| `this[int]`                        | `this[int]`                        |
| `Reverse()`                        |                                    |
| `Reverse(int, int)`                |                                    |
| `RotateLeft(int)`                  |                                    |
| `RotateLeft(int, int, int)`        |                                    |
| `RotateRight(int)`                 |                                    |
| `RotateRight(int, int, int)`       |                                    |

## Extending `ObservableList<T>`

`ObservableList<T>` exposes both the underlying `Items` list and methods for publishing `INotifyCollectionChanged`
events. The example below demonstrates how to extend `ObservableList<T>` with custom operations.

```csharp
public static class ObservableListExtensions
{
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
}
```

More examples can be found in [ObservableListExtensions](Pylon.ObservableList/ObservableListExtensions.cs).

## Building

Build using `dotnet`.

```shell
dotnet build --configuration release
```

Test using `dotnet`.

```shell
dotnet test
```