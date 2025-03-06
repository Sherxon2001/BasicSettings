namespace BasicSettings.DataLayer.Entities
{
    public class RoleProfiles
    {
        public int TaskId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public SystemTasks SystemTasks { get; set; }
        [ForeignKey(nameof(RoleId))]
        public ApplicantRole Role { get; set; }
    }
}
