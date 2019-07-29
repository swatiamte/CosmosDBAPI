using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebAPICosmosDB.Models;

namespace WebAPICosmosDB.Controllers
{
   // [RoutePrefix("api/TimeSeries")]
    public class TimeSeriesController : ApiController
    {
        [HttpGet]
        public async Task<Dictionary<string, DateTime>> GetAsync()
        {
            Dictionary<string, DateTime> lstResponse = new Dictionary<string, DateTime>();
            lstResponse = await TimeSeriesInsightsLogic.SampleAsync();
            return lstResponse;
        }
        [HttpPost]
        public async Task<IEnumerable<TimeSeriesResponse>> getEventDataAsync([FromBody]TimeSeriesRequest TimeSeriesRequestData)
        {

            IEnumerable<TimeSeriesResponse> responseContent =  await TimeSeriesInsightsLogic.getEventData(TimeSeriesRequestData.from, TimeSeriesRequestData.to, TimeSeriesRequestData.TagName);
            return responseContent;
        }
    }
}
 