namespace Pylon.ObservableList;

public static class MathUtilities
{
    /// <summary>
    /// Calculate <see cref="a"/> modulo <see cref="b"/>.
    /// </summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <returns>The remainder.</returns>
    public static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}