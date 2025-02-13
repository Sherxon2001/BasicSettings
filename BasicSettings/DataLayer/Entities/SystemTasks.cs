using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Entities
{
    public class SystemTasks
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }

        public SystemTasks? Parent { get; set; }

        [ForeignKey(nameof(ParentId))]
        public ICollection<SystemTasks>? Children { get; set; }

        public string? Type { get; set; }
    }
}
