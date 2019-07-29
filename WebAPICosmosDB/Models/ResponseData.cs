using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICosmosDB.Models
{
    public class ResponseData
    {
        public string connectionDeviceId;
        public double availability;
        public double Performance;//Hardcoded
        public double Quality; // Hardcoded
        public double MTTR;// hardcode
        public double MTBF; //Hardcoded
       
        public ResponseData(string connectionDeviceId, double availability, double Performance, double Quality, double MTTR,double MTBF)
        {
            this.connectionDeviceId = connectionDeviceId;
            this.availability = availability;
            this.Performance= Performance;
            this.Quality= Quality;
            this.MTTR= MTTR;
            this.MTBF= MTBF;



        }
    }
}