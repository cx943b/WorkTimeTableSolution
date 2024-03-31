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
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class WorkerModelJsonConverter : JsonConverter<WorkerModel>
    {
        static readonly PropertyInfo[] _orderedPropInfos = typeof(WorkerModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(p => p.Name)
            .ToArray();

        public override WorkerModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            WorkerModel newWorker = new WorkerModel();
            string? propName = String.Empty;

            foreach(var propInfo in _orderedPropInfos)
            {
                reader.Read();
                propName = reader.GetString();

                if(String.IsNullOrEmpty(propName))
                    throw new JsonException("PropertyName is null or empty");

                reader.Read();

                if(propInfo.CanWrite)
                {
                    propInfo.SetValue(newWorker, JsonSerializer.Deserialize(ref reader, propInfo.PropertyType, options));
                }
                else
                {
                    if(String.Compare(propName, nameof(WorkerModel.WorkTimes), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var workTimes = JsonSerializer.Deserialize<IEnumerable<WorkTimeModel>>(ref reader, options);
                        if (workTimes == null)
                            throw new JsonException($"Unable to deserialize {propName}");

                        newWorker.AddWorkTimes(workTimes);
                    }
                }
            }

            reader.Read();
            return newWorker;
        }

        public override void Write(Utf8JsonWriter writer, WorkerModel value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach(var propInfo in _orderedPropInfos)
            {
                writer.WritePropertyName(propInfo.Name);
                JsonSerializer.Serialize(writer, propInfo.GetValue(value), options);
            }

            writer.WriteEndObject();
        }
    }
}