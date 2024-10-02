using Avalonia;
using Avalonia.Headless;
using Pylon.ObservableList.UnitTests;

[assembly: AvaloniaTestApplication(typeof(TestApplicationBuilder))]

namespace Pylon.ObservableList.UnitTests;

public class TestApplicationBuilder
{
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<TestApplication>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
    }
}