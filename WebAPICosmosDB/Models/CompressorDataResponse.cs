using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICosmosDB.Models
{
    
   
    public class CompressorDataResponse
    {
        public string connectionDeviceId;
        public string status;
        public string sourceTimestamp;
        public CompressorDataResponse(string connectionDeviceId, string status, string sourceTimestamp)
        {
            this.connectionDeviceId = connectionDeviceId;
            this.status = status;
            this.sourceTimestamp = sourceTimestamp;

        }
        //public string CompressorName { get; set; }
        //public string Status { get; set; }
        //public DateTime SourceTimestamp { get; set; }

    }
}