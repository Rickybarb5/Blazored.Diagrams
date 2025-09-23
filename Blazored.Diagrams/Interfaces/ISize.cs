namespace Blazored.Diagrams.Interfaces;

/// <summary>
///     Properties that define the size of a model.
/// </summary>
public interface ISize
{
    /// <summary>
    ///     Width of the model.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    ///     Height of the model.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    ///     Sets the width and height of the model.
    ///     Triggers the SizeChanged events.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    void SetSize(int width, int height);
}