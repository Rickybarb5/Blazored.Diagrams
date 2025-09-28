namespace Blazored.Diagrams.Sandbox.Pages.Examples.CalculatorExample;

public interface INumberOutput
{
    public decimal? NumberOutput { get; set; }
    event Action<decimal?> OnNumberChanged;
    void NotifyNumberChanged();
}