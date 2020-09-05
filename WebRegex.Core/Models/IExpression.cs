namespace WebRegex.Core.Models
{
    public interface IExpression
    {
        int Id { get; set; }
        string Name { get; set; }
        int ProfileId { get; set; }
        string Regex { get; set; }
        bool IsIdentifier { get; set; }
        int IsIdentifierBit { get; set; }
    }
}