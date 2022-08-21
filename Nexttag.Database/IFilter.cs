namespace Nexttag.Database
{
    public interface IFilter
    {
        string Field { get; }
        string Variable { get; }
        object Value { get; }
        OperatorType Operator { get; }
    }
}