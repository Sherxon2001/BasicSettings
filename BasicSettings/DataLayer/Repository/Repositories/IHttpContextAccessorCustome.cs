using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSettings.DataLayer.Repository.Repositories
{
    public interface IHttpContextAccessorCustome
    {
        IHttpContextAccessor HttpContextAccessor { get; }
        int GetUserId();
    }

    public class HttpContextAccessorCustome : IHttpContextAccessorCustome
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextAccessorCustome(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

        public int GetUserId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
