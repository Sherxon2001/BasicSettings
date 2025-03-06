namespace BasicSettings.DataLayer.Entities
{
    public class UsersRoles
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicantUser User { get; set; }
        [ForeignKey(nameof(RoleId))]
        public ApplicantRole Role { get; set; }
    }
}
