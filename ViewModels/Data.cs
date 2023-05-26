using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INF354ReportHomework.ViewModels
{
    public class Data
    {
        public class DateVM
        {
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
            public int productCategoryID { get; set; }
        }

        public class AuthVM
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
        }
    }
}
