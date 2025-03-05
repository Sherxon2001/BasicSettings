namespace BasicSettings.Essentials.DateTimeConverter
{
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly string[] _readFormats = { "dd.MM.yyyy H:mm:ss", "dd.MM.yyyy" };
        private readonly string _writeFormatWithTime = "yyyy-MM-ddTHH:mm:ss";
        private readonly string _writeFormatWithoutTime = "yyyy-MM-dd";

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var dateString = reader.Value.ToString();
                foreach (var format in _readFormats)
                {
                    if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                }
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                var format = value.Value.TimeOfDay.TotalSeconds == 0 ? _writeFormatWithoutTime : _writeFormatWithTime;
                writer.WriteValue(value.Value.ToString(format));
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
