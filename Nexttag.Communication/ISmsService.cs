namespace Nexttag.Communication;

public interface ISmsService
{
    Task<bool> SendSmsAsync(string phone, string msg);

}