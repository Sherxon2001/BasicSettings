namespace BasicSettings.Essentials.DateTimeConverter
{
    public class CustomDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime?>
    {
        private readonly string[] _readFormats = { "dd.MM.yyyy H:mm:ss", "dd.MM.yyyy" };
        private readonly string _writeFormatWithTime = "dd.MM.yyyy";
        private readonly string _writeFormatWithoutTime = "dd.MM.yyyy";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                var format = value.Value.TimeOfDay.TotalSeconds == 0 ? _writeFormatWithoutTime : _writeFormatWithTime;
                writer.WriteStringValue(value.Value.ToString(format));
            }
            else
            {
                writer.WriteNullValue();
            }
        }

    }
}
