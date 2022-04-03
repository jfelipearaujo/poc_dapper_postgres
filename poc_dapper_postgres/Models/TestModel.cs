using System;

namespace poc_dapper_postgres.Models
{
    public class TestModel
    {
        public Guid Id { get; set; }
        public int NumDate { get; set; }
        public decimal NumValue { get; set; }

        public Guid TestParentModelId { get; set; }
    }
}
