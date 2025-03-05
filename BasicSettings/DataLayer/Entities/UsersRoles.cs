namespace BasicSettings.DataLayer.Entities
{
    public class UsersRoles
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public CustomeIdentityUser User { get; set; }
        [ForeignKey(nameof(RoleId))]
        public CustomeIdentityRole Role { get; set; }
    }
}
