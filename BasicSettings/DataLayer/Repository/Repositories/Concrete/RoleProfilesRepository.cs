using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Repository.Repositories.Concrete
{
    public class RoleProfilesRepository : RepositoryBase<RoleProfiles>, IRoleProfilesRepository
    {
        public RoleProfilesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
