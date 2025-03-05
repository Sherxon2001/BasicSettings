namespace BasicSettings.Extensions
{
    public static class SystemExtensions
    {
        public static string ToUtf8(this string message)
        {
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            string utf8Message = System.Text.Encoding.UTF8.GetString(messageBytes);
            return utf8Message;
        }

        public static string SplitPhoneNumber(this string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return string.Empty;

            // Faqat raqamlarni ajratib olish
            var cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Formatlash
            if (cleanedNumber.Length == 12) // +998 XXX XXX XXX            
                return $"+{cleanedNumber.Substring(0, 3)} {cleanedNumber.Substring(3)}";
            else
                throw new ArgumentException($"Telefon raqami noto'g'ri formatda. Phone {phoneNumber}");
        }

        public static DateTime ToDateFormatForZags(this string? dateTime)
        {
            if (DateTime.TryParseExact(dateTime, "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static string ToDateFormatAsStringForPovertyRegistry(this string? dateTime)
        {
            if (DateTime.TryParseExact(dateTime, "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd");
            }
            else
            {
                throw new ArgumentException("Date format is incorrect.");
            }
        }

        public static async Task<IPageCollection<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Essentials.PageValidation.PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static DateTime ToDateFormatFromGspDateFormat(this string gspDateFormat)
        {
            if (string.IsNullOrEmpty(gspDateFormat))
                return DateTime.MinValue;

            try
            {
                return DateTime.ParseExact(gspDateFormat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static (string PassportSeriaId, string PassportNumber) SplitDocument(this string document)
        {
            if (document?.Length < 2 || string.IsNullOrEmpty(document))
                return ("", "");
            return (document.Substring(0, 2), document.Substring(2));
        }

        public static void ChangeTrackerExtensions(ref string document, Dictionary<string, string> keyValuePairs)
        {
            var stringBuilder = new StringBuilder(document);
            foreach (var keyValuePair in keyValuePairs)
            {
                stringBuilder.Replace(keyValuePair.Key, keyValuePair.Value);
            }
            document = stringBuilder.ToString();
        }
    }
}
