using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Entities
{
    public class UsersRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public CustomeIdentityUser User { get; set; }
        [ForeignKey(nameof(RoleId))]
        public CustomeIdentityRole Role { get; set; }
    }
}
