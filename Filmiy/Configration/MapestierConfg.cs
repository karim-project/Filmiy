using Mapster;

namespace Filmiy.Configration
{
    public static class MapestierConfg
    {
        public static void RegisterMapsterConfig(this IServiceCollection services)
        {
            TypeAdapterConfig<ApplicationUser, ApplicationUserVM>
                    .NewConfig()
                    .Map(d => d.FullName, s => $"{s.FirstName} {s.LastName}")
                    .TwoWays();
        }
    }
}
