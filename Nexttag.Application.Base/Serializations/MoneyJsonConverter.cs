using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nexttag.Application.Base.Serializations;

public class MoneyJsonConverter : JsonConverter<Money>
{
    public override Money Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => new Money() { Value = reader.GetDecimal() };

    public override void Write(
        Utf8JsonWriter writer,
        Money money,
        JsonSerializerOptions options) => writer.WriteStringValue(money.ToString());

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(decimal) || typeToConvert == typeof(Money);
    }
}