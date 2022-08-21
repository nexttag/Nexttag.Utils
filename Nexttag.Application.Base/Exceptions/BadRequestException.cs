namespace Nexttag.Application.Base.Exceptions;

[Serializable]
public class BadRequestException : Exception
{
    public BadRequestException(string message, IEnumerable<Field> invalidFields) : base(message)
    {
        InvalidFields = invalidFields;
    }

    public IEnumerable<Field> InvalidFields { get; }

}