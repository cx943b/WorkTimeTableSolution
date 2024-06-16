using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class WorkerModelJsonConverter : JsonConverter<WorkerModel>
    {
        static readonly PropertyInfo[] _orderedPropInfos = typeof(WorkerModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null)
            .OrderBy(p => p.Name)
            .ToArray();

        public override WorkerModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            WorkerModel newWorker = new WorkerModel();
            string? propName = String.Empty;
            PropertyInfo? propInfo = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if(reader.TokenType == JsonTokenType.PropertyName)
                {
                    propName = reader.GetString();
                }
                else if(reader.TokenType == JsonTokenType.StartArray)
                {
                    var workTimes = JsonSerializer.Deserialize<IEnumerable<WorkTimeModel>>(ref reader, options);
                    if (workTimes == null)
                        throw new JsonException($"NotFound: {propName}");

                    foreach (var workTime in workTimes)
                        newWorker.AddWorkTime(workTime);
                }
                else
                {
                    propInfo = _orderedPropInfos.FirstOrDefault(pi => String.Compare(pi.Name, propName, StringComparison.OrdinalIgnoreCase) == 0);
                    if (propInfo == null)
                        throw new JsonException($"Unable to deserialize {propName}");

                    propInfo.SetValue(newWorker, JsonSerializer.Deserialize(ref reader, propInfo.PropertyType, options));
                }
            }

            return newWorker;
        }

        public override void Write(Utf8JsonWriter writer, WorkerModel worker, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach(var propInfo in _orderedPropInfos)
            {
                writer.WritePropertyName(propInfo.Name);
                JsonSerializer.Serialize(writer, propInfo.GetValue(worker), options);
            }

            writer.WriteStartArray(nameof(worker.WorkTimes));
            foreach (var workTime in worker.WorkTimes)
            {
                JsonSerializer.Serialize(writer, workTime, options);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}