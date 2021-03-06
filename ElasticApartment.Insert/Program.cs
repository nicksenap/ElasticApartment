using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElasticApartment.Model;
using Microsoft.Extensions.Configuration;
using Nest;
using static Newtonsoft.Json.JsonConvert;

namespace ElasticApartment.Insert
{
    internal class Program
    {
        private static async Task Main()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);
            var configuration = builder.Build();
            var url = configuration["elasticsearch:url"];
            var user = configuration["elasticsearch:user"];
            var pass = configuration["elasticsearch:password"];
            var propertyPath = configuration["LocalFile:propertiespath"];
            var mgmtsPath = configuration["LocalFile:mgmtspath"];

            var node = new Uri(url);
            var settings = new ConnectionSettings(node);
            settings.BasicAuthentication(user, pass);
            var client = new ElasticClient(settings);

            var properties = await ReadPropertiesFromJson(propertyPath);
            var mgmts = await ReadManagementsFromJson(mgmtsPath);

            await client.Indices.CreateAsync("property", index => index.Map<Property>(x => x.AutoMap()));
            await client.Indices.CreateAsync("mgmt", index => index.Map<Management>(x => x.AutoMap()));

            await client.BulkAsync(b => b.Index("property").IndexMany(properties));
            await client.BulkAsync(b => b.Index("mgmt").IndexMany(mgmts));

            Console.WriteLine("Done inserting!");
        }

        private static async Task<IEnumerable<Property>> ReadPropertiesFromJson(string path)
        {
            return DeserializeObject<List<PropertyItem>>(await File.ReadAllTextAsync(path))!
                .Select(x => x.Property).GroupBy(x => x.PropertyId).Select(grp => grp.First());
        }

        private static async Task<IEnumerable<Management>> ReadManagementsFromJson(string path)
        {
            return DeserializeObject<List<ManagementItem>>(await File.ReadAllTextAsync(path))!
                .Select(x => x.Management).GroupBy(x => x.ManagementId).Select(grp => grp.First());
        }
    }
}