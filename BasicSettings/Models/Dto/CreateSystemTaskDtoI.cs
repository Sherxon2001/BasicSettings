namespace BasicSettings.Models.Dto
{
    public class CreateSystemTaskDtoI
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        [RegularExpression(@"\b(ACTION|MENU|SUB_MENU)\b", ErrorMessage = "ACTION, MENU, SUB_MENU so'zlarini qabul qiladi.")]
        public string Type { get; set; }
    }
}
