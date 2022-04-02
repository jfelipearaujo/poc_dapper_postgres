using Dapper.FluentMap.Dommel.Mapping;

using poc_dapper_postgres.Models;

namespace poc_dapper_postgres.Mapping
{
    public class TestModelMap : DommelEntityMap<TestModel>
    {
        public TestModelMap()
        {
            ToTable("tbltest");

            Map(x => x.Id).ToColumn("id").IsKey().IsIdentity();
            Map(x => x.NumDate).ToColumn("num_date");
            Map(x => x.NumValue).ToColumn("num_vlr");
        }
    }
}
