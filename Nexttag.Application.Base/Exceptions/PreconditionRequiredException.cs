namespace Nexttag.Application.Base.Exceptions;

[Serializable]
public class PreconditionRequiredException : Exception
{
    public PreconditionRequiredException() { }
    public PreconditionRequiredException(string message) : base(message) { }
    public PreconditionRequiredException(string message, Exception inner) : base(message, inner) { }
    protected PreconditionRequiredException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}