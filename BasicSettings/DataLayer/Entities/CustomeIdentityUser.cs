namespace BasicSettings.DataLayer.Entities
{
    public class CustomeIdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public int AccessFailedCount { get; set; } = 0;
        public bool IsActive { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
