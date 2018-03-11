using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DataAccess.Entities
{
    public class Todo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
