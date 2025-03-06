namespace BasicSettings.Models
{
    public class ApplicantUserDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }

        public static explicit operator ApplicantUserDto(ApplicantUser user)
        {
            if(user == null)
                return null;

            return new ApplicantUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                RegionId = user.RegionId,
                DistrictId = user.DistrictId
            };
        }
    }
}
