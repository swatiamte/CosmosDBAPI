using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICosmosDB.Models
{
    public class TimeSeriesResponse
    {
        public string date;
        public string value;

        public TimeSeriesResponse(string date, string value)
        {
            this.date = date;
            this.value = value;


        }
    }
}