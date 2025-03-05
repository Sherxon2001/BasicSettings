namespace BasicSettings.Models.Dto
{
    public class RoleProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<RoleProfileMenuDto> Menus { get; set; } = new HashSet<RoleProfileMenuDto>();
    }

    public class RoleProfileMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public HashSet<RoleProfileSubMenuDto> SubMenus { get; set; } = new HashSet<RoleProfileSubMenuDto>();
        public HashSet<RoleProfileActionDto> Actions { get; set; } = new HashSet<RoleProfileActionDto>();
    }
    public class RoleProfileActionDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
    }
    public class RoleProfileSubMenuDto
    {
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
    }
}
