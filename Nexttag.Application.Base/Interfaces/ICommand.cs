namespace Nexttag.Application.Base.Interfaces;

public interface ICommand<Request, Response>
{
    public Response Execute(Request request);
}