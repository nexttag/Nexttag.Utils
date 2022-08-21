using System.Globalization;

namespace Nexttag.Application.Base.Culture;

public static class CultureHelper
{
    private static CultureInfo _ptBR = CultureInfo.GetCultureInfo("pt-BR");
    private static CultureInfo _enUS = CultureInfo.GetCultureInfo("en-US");
    public static CultureInfo PT_BR => _ptBR;
    public static CultureInfo EN_US => _enUS;
}