using System;
using System.Collections.Generic;

namespace poc_dapper_postgres.Models
{
    public class TestParentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }

        public List<TestModel> TestModels { get; set; }
    }
}
