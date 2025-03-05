namespace BasicSettings.Models
{
    public class CustomeIdentityUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
    }
}
