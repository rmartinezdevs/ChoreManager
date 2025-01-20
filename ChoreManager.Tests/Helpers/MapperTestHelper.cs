using AutoMapper;
using ChoreManager.Application.AutoMapper;

public static class MapperTestHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        return config.CreateMapper();
    }
}