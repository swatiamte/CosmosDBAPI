namespace WebAPICosmosDB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Newtonsoft.Json;
    using WebAPICosmosDB.Models;

    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static DocumentClient client;

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
        public static async Task<DateTime> GetLastAddedRecordDate()
        {
            List<T> results = new List<T>();

            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
            UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), string.Format("SELECT top 1 * FROM c order by c._ts desc"),
            new FeedOptions { MaxItemCount = -1 })
            .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            string json = JsonConvert.SerializeObject(results);
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CompressorData> finalResult = jsSerializer.Deserialize<List<CompressorData>>(json);

            return Convert.ToDateTime(finalResult[0].value.SourceTimestamp);
        }
        public static async Task<List<ResponseData>> GetItemsAsync()
        {
            CommonFunctions com = new CommonFunctions();
            List<CompressorDataResponse> lst = new List<CompressorDataResponse>();
            string status = "";
            List<T> results = new List<T>();
           
                IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), string.Format("SELECT * from c where c.DisplayName='Status'"),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();
           
           

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }
            string json = JsonConvert.SerializeObject(results);
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CompressorData> finalResult = jsSerializer.Deserialize<List<CompressorData>>(json);
            foreach (var a in finalResult)
            {
                if (a.DisplayName == "Status")
                {
                    status = com.ConvertDecToBinary(Convert.ToInt32(a.value.value));
                    if (status == "0")
                        lst.Add(new CompressorDataResponse(a.ioTHub.ConnectionDeviceGenerationId, "Not Ready", a.value.SourceTimestamp));
                    else

                        lst.Add(new CompressorDataResponse(a.ioTHub.ConnectionDeviceGenerationId, status.Substring((status.Length - 2), 1) == "1" ? "Ready" : "Not Ready", a.value.SourceTimestamp));

                }
            }

            var Compquery = lst.GroupBy(x => x.connectionDeviceId).Select(g => g.OrderBy(x => x.sourceTimestamp)).SelectMany(g => g).ToList();


            double availability = 0, availabilityPercentage = 0;
            Dictionary<string, string> d = new Dictionary<string, string>();
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            DateTime date3 =await GetLastAddedRecordDate();

            for (int i = 0; i < Compquery.Count; i++)
            {
                date2 = Convert.ToDateTime(Compquery[0].sourceTimestamp);

                if (i == 0)
                {
                    date1 = Convert.ToDateTime(Compquery[i].sourceTimestamp);

                }
                else
                {


                    if (Compquery[i - 1].status != "Not Ready")
                    {
                        TimeSpan time = Convert.ToDateTime(Compquery[i].sourceTimestamp) - date1;

                        availability = availability + (time.Days * 24 * 60 + time.Hours * 60 + time.Minutes);
                        date1 = Convert.ToDateTime(Compquery[i].sourceTimestamp);
                    }
                    else
                    {
                        date1 = Convert.ToDateTime(Compquery[i].sourceTimestamp);
                    }
                }



            }
            TimeSpan time1 = date3 - date2;
            double TotalTime = (time1.Days * 24 * 60 + time1.Hours * 60 + time1.Minutes);
            availabilityPercentage = availability / TotalTime * 100;
            List<ResponseData> lstResponse = new List<ResponseData>();

            lstResponse.Add(new ResponseData(Compquery[0].connectionDeviceId, Math.Round(availabilityPercentage, 2),76,89,77,45));
            string jsonResponseData = JsonConvert.SerializeObject(lstResponse);

            return lstResponse;
           
        }

       

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), string.Format("SELECT * FROM c where c.DisplayName='Status'"),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }


            return results;

           
    }
        public static async Task<T> GetSingleItemAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();
            List<T> results = new List<T>();
            results.AddRange(await query.ExecuteNextAsync<T>());
            return results.SingleOrDefault();
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
            try
            {
                client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);

                CreateDatabaseIfNotExistsAsync().Wait();
                CreateCollectionIfNotExistsAsync().Wait();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}