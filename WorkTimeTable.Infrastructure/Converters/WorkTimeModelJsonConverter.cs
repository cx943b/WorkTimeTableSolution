using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class WorkTimeModelJsonConverter : JsonConverter<WorkTimeModel>
    {
        static readonly PropertyInfo[] _orderedPropInfos = typeof(WorkTimeModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(p => p.Name)
            .ToArray();

        public override WorkTimeModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            WorkTimeModel newWorkTime = new WorkTimeModel();
            string? propName = String.Empty;
            PropertyInfo? propInfo = null;

            do
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                    continue;
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                propName = reader.GetString();
                if (String.IsNullOrEmpty(propName))
                    throw new JsonException($"NullRef: JsonPropName");

                propInfo = _orderedPropInfos.FirstOrDefault(p => String.Compare(p.Name, propName, StringComparison.OrdinalIgnoreCase) == 0);
                if (propInfo == null)
                    throw new JsonException($"NotFound: {nameof(propName)}");

                reader.Read();

                if(propInfo.CanWrite)
                {
                    propInfo.SetValue(newWorkTime, JsonSerializer.Deserialize(ref reader, propInfo.PropertyType, options));
                }
                else 
                {
                    switch(propName)
                    {
                        case nameof(WorkTimeModel.StartWorkTime):
                            {
                                DateTime? oStartWorkTime = JsonSerializer.Deserialize(ref reader, propInfo.PropertyType, options) as DateTime?;
                                if (!oStartWorkTime.HasValue)
                                    throw new JsonException($"Unable to deserialize {propName}");

                                DateTime startWorkTime = oStartWorkTime.Value;
                                newWorkTime.Year = startWorkTime.Year;
                                newWorkTime.Month = startWorkTime.Month;
                                newWorkTime.Day = startWorkTime.Day;
                                newWorkTime.Hour = startWorkTime.Hour;
                                newWorkTime.Minute = startWorkTime.Minute;

                                break;
                            }
                    }
                }
            } while (reader.Read());

            return newWorkTime;
        }

        public override void Write(Utf8JsonWriter writer, WorkTimeModel value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var propInfo in _orderedPropInfos)
            {
                if (propInfo.CustomAttributes.Any(att => att.AttributeType == typeof(JsonIgnoreAttribute)))
                    continue;

                writer.WritePropertyName(propInfo.Name);
                JsonSerializer.Serialize(writer, propInfo.GetValue(value), options);
            }

            writer.WriteEndObject();
        }
    }
}