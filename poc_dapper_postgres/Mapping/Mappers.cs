using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace poc_dapper_postgres.Mapping
{
    public static class Mappers
    {
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TestModelMap());

                config.ForDommel();
            });
        }
    }
}
