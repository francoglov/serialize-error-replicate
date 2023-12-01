using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnumSerializationExample
{
    public enum ContentType
    {
        [Description("Studies")]
        Studies = 1,

        [Description("Papers")]
        Papers = 2,

        [Description("Books")]
        Books = 3,
    }

    [DataContract]
    public class Content
    {
        [DataMember]
        public ContentType Type { get; set; }
    }

    class Program
    {
        static void Main()
        {

            //There is no 0 in the Enum. No Error Here
            string responseTextint = "{\"Type\":0}";
            try
            {
                DeserializeContent(responseTextint);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"JsonSerializationException: {ex.Message}");
            }

            //Change it to String.. No Error
            string responseTextString = "{\"Type\":\"0\"}";
            try
            {
                DeserializeContent(responseTextString);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"JsonSerializationException: {ex.Message}");
            }

            //Change it to Letter 'O'. Error occurs but a different one
            string responseTextO = "{\"Type\":\"O\"}";
            try
            {
                DeserializeContent(responseTextO);
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"JsonSerializationException: {ex.Message}");
            }

            // Serialize an object with an invalid enum value = Error here.
            // This is the only Scenario that I can think of to 
            // replicate the error.
            try
            {
                var content = new Content { Type = (ContentType)Enum.Parse(typeof(ContentType), "0") };

                var serializer = new DataContractSerializer(typeof(Content));

                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, content);
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine($"SerializationException: {ex.Message}");
            }
        }

        static void DeserializeContent(string json)
        {
            var searchResponse = JsonConvert.DeserializeObject<Content>(json);
        }
    }
}
