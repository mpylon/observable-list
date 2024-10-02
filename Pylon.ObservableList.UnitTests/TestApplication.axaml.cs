using Avalonia;
using Avalonia.Markup.Xaml;

namespace Pylon.ObservableList.UnitTests;

public class TestApplication : Application
{
    public TestApplication()
    {
        AvaloniaXamlLoader.Load(this);
    }
}