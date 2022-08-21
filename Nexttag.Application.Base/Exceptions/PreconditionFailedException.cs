namespace Nexttag.Application.Base.Exceptions;

[Serializable]
public class PreconditionFailedException : Exception
{
    public PreconditionFailedException()
    {
    }

    public PreconditionFailedException(string message) : base(message)
    {
    }

    public PreconditionFailedException(string message, Exception inner) : base(message, inner)
    {
    }

    protected PreconditionFailedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}