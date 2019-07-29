using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICosmosDB.Models
{
    public class TimeSeriesRequest
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string TagName { get; set; }


    }
}