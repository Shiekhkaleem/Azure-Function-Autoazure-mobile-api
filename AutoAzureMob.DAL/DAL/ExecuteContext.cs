using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class ExecuteContext
    {
        public SqlConnection Connection { get; set; }

        public SqlTransaction Transaction { get; set; }
    }
}
