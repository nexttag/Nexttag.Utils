namespace Nexttag.Communication;

public class NotificationAllowlist<T> : INotificationAllowlist<T>
{
    public String[] AllowList { get; set; }
    public String Override { get; set; }

    public NotificationAllowlist(String[] allowList, String overrideAddress)
    {
        this.AllowList = allowList;
        this.Override = overrideAddress;
    }

    public String VerifyAndGetRecipient(string recipient)
    {
        if (AllowList == null || AllowList.Count() == 0)
        {
            return null;
        }
        else if (!AllowList.Contains("*")
                 && !AllowList.Contains(recipient))
        {
            if (string.IsNullOrEmpty(Override))
            {
                return null;
            }
            return Override;
        }
        return recipient;
    }
}