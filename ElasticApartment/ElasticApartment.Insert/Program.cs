using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElasticApartment.Model;
using Nest;
using Newtonsoft.Json;

namespace ElasticApartment.Insert
{
    internal class Program
    {
        private static async Task Main()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            var properties = JsonConvert
                .DeserializeObject<List<PropertyItem>>(File.ReadAllText("C:\\Users\\nsong\\Downloads\\properties.json"))
                .Select(x => x.Property);
            var mgmts = JsonConvert
                .DeserializeObject<List<ManagementItem>>(File.ReadAllText("C:\\Users\\nsong\\Downloads\\mgmt.json"))
                .Select(x => x.Management);

            await client.BulkAsync(b => b.Index("property").IndexMany(properties));
            await client.BulkAsync(b => b.Index("mgmt").IndexMany(mgmts));

            Console.WriteLine("Done inserting!");
        }
    }
}