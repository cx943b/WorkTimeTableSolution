
using CommunityToolkit.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Markup;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure;


public class SolidColorBrushJsonConverter : JsonConverter<SolidColorBrush>
{
    public override SolidColorBrush? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        string? propName = reader.GetString(); // PropertyName

        reader.Read();
        Color color = (Color)ColorConverter.ConvertFromString(reader.GetString());

        reader.Read();
        reader.GetString(); // PropertyName

        reader.Read();
        double opacity = reader.GetDouble();

        reader.Read();      // EndOfObject

        SolidColorBrush brush = new SolidColorBrush();
        brush.Opacity = opacity;
        brush.Color = color;

        return brush;
    }

    public override void Write(Utf8JsonWriter writer, SolidColorBrush value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName($"{nameof(SolidColorBrush.Color)}");
        writer.WriteStringValue(value.Color.ToString());

        writer.WritePropertyName($"{nameof(SolidColorBrush.Opacity)}");
        writer.WriteNumberValue(value.Opacity);
        writer.WriteEndObject();
    }
}