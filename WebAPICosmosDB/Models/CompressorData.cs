using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace WebAPICosmosDB.Models
{
    public class CompressorData
    {
      /*  [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }
       */
                public class Value
                {
                    public int value { get; set; }
                    public string SourceTimestamp { get; set; }
                }

                public class IoTHub
                {
                    public object MessageId { get; set; }
                    public object CorrelationId { get; set; }
                    public string ConnectionDeviceId { get; set; }
                    public string ConnectionDeviceGenerationId { get; set; }
                    public DateTime EnqueuedTime { get; set; }
                    public object StreamId { get; set; }
                }

               
                    public string NodeId { get; set; }
                    public string ApplicationUri { get; set; }
                    public string DisplayName { get; set; }
                    public Value value { get; set; }
                    public DateTime EventProcessedUtcTime { get; set; }
                    public int PartitionId { get; set; }
                    public DateTime EventEnqueuedUtcTime { get; set; }
                    public IoTHub ioTHub { get; set; }
                    public string id { get; set; }
                    public string _rid { get; set; }
                    public string _self { get; set; }
                    public string _etag { get; set; }
                    public string _attachments { get; set; }
                    public int _ts { get; set; }
                
    }
}