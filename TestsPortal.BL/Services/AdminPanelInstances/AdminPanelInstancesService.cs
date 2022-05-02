using Microsoft.Extensions.Configuration;

namespace TestsPortal.BL.Services.AdminPanelInstances
{
    public class AdminPanelInstancesService : IAdminPanelInstancesService
    {
        private const string AdminPanelInstancesConfigKey = "AdminPanelInstances";
        private readonly IConfiguration _configuration;

        public AdminPanelInstancesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetInstanceUrl(string instanceKey)
        {
            var section = _configuration.GetSection(AdminPanelInstancesConfigKey);
            return section.GetChildren().Where(x => x.Key == instanceKey).Single().Value;
        }
    }
}
