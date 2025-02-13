using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Entities
{
    public class RoleProfiles
    {
        public int TaskId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public SystemTasks SystemTask { get; set; }
        [ForeignKey(nameof(RoleId))]
        public CustomeIdentityRole Role { get; set; }
    }
}
