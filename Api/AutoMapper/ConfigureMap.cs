using AutoMapper;

namespace Application.AutoMapper
{
    public static class ConfigureMap
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DataMappingProfile>();
            });
        }
    }
}
