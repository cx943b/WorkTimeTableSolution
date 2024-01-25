using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class WorkerModelJsonConverter : JsonConverter<WorkerModel>
    {
        public override WorkerModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            reader.GetString(); // PropertyName: Id

            reader.Read();
            int id = reader.GetInt32();

            reader.Read();
            reader.GetString(); // PropertyName: Name

            reader.Read();
            string? name = reader.GetString();
            if(String.IsNullOrEmpty(name))
                throw new JsonException("Name is null or empty");

            reader.Read();
            reader.GetString(); // PropertyName: FixedWorkWeeks

            reader.Read();
            DayOfWeekFlag fixedWorkWeeks = (DayOfWeekFlag)reader.GetInt32();

            reader.Read();
            reader.GetString(); // PropertyName: Brush

            reader.Read();
            SolidColorBrush? brush = JsonSerializer.Deserialize<SolidColorBrush>(ref reader, options);
            if (brush == null)
                throw new JsonException("Brush is null");

            reader.Read();
            reader.GetString(); // PropertyName: WorkTimes

            WorkTimeModel[]? workTimes = JsonSerializer.Deserialize<WorkTimeModel[]>(ref reader, options);
            reader.Read();      // EndOfObject

            WorkerModel worker = new WorkerModel(id, name, brush, fixedWorkWeeks, workTimes);
            return worker;
        }

        public override void Write(Utf8JsonWriter writer, WorkerModel value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(WorkerModel.Id), value.Id);
            writer.WriteString(nameof(WorkerModel.Name), value.Name);
            writer.WriteNumber(nameof(WorkerModel.FixedWorkWeeks), (int)value.FixedWorkWeeks);

            writer.WritePropertyName(nameof(WorkerModel.Brush));
            JsonSerializer.Serialize<SolidColorBrush>(writer, value.Brush, options);

            writer.WriteStartArray(nameof(WorkerModel.WorkTimes));
            foreach(var workTime in value.WorkTimes.OfType<WorkTimeModel>())
            {
                JsonSerializer.Serialize<WorkTimeModel>(writer, workTime, options);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
