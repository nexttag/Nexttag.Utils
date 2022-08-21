namespace Nexttag.Communication;

public interface INotificationAllowlist<T>
{
    String VerifyAndGetRecipient(string recipient);
}