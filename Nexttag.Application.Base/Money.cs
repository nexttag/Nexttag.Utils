using System.Globalization;
using System.Text.Json.Serialization;
using Nexttag.Application.Base.Serializations;

namespace Nexttag.Application.Base;

[JsonConverter(typeof(MoneyJsonConverter))]
public class Money
{
    public Money()
    {
        Localization = CultureInfo.GetCultureInfo("pt-BR");
    }

    public CultureInfo Localization { get; set; }
    public decimal? Value { get; set; }

    public static explicit operator decimal(Money d) => d.Value ?? 0.00M;

    public static explicit operator Money(decimal d) => new Money() { Value = d };

    public static explicit operator Money(decimal? d) => new Money() { Value = d };

    public override string ToString()
    {
        return Value.HasValue ? ((FormattableString)$"R$ {Value:N2}").ToString(Localization) : "R$ -";
    }
}