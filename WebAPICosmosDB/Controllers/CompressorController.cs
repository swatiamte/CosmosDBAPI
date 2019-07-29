using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPICosmosDB.Models;

namespace WebAPICosmosDB.Controllers
{
    [RoutePrefix("api/Compressor")]
    public class CompressorController : ApiController
    {
        // GET: Compressor
        [HttpGet]
        public async Task<List<ResponseData>> GetAsync()
        {

            List<ResponseData> lstResponse = new List<ResponseData>();
            lstResponse = await DocumentDBRepository<CompressorData>.GetItemsAsync();
            return lstResponse;


        }



        [HttpPost]
        public async Task<CompressorData> CreateAsync([System.Web.Http.FromBody] CompressorData CompressorData)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<CompressorData>.CreateItemAsync(CompressorData);
                return CompressorData;
            }
            return null;
        }
        public async Task<string> Delete(string uid)
        {
            try
            {
                CompressorData item = await DocumentDBRepository<CompressorData>.GetSingleItemAsync(d => d.id == uid);
                if (item == null)
                {
                    return "Failed";
                }
                await DocumentDBRepository<CompressorData>.DeleteItemAsync(item.id);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<CompressorData> Put(string uid, [System.Web.Http.FromBody] CompressorData CompressorData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompressorData item = await DocumentDBRepository<CompressorData>.GetSingleItemAsync(d => d.id == uid);
                    if (item == null)
                    {
                        return null;
                    }
                    CompressorData.id = item.id;
                    await DocumentDBRepository<CompressorData>.UpdateItemAsync(item.id, CompressorData);
                    return CompressorData;
                }
                return null; ;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
