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
            var node = new Uri("https://search-apartment-data-iwpi2negcpu47wdiyo5g5a5vny.eu-north-1.es.amazonaws.com/");
            var settings = new ConnectionSettings(node);
            settings.BasicAuthentication("foo", "Foobar123*");
            var client = new ElasticClient(settings);

            var properties = JsonConvert
                .DeserializeObject<List<PropertyItem>>(File.ReadAllText("C:\\dev\\apartmentData\\properties.json"))!
                .Select(x => x.Property).GroupBy(x => x.PropertyId).Select(grp => grp.First());
            var mgmts = JsonConvert
                .DeserializeObject<List<ManagementItem>>(File.ReadAllText("C:\\dev\\apartmentData\\mgmt.json"))!
                .Select(x => x.Management).GroupBy(x => x.ManagementId).Select(grp => grp.First());

            await client.Indices.CreateAsync("property",
                index => index.Map<Property>(x => x.AutoMap())
                    .Settings(ss =>
                        ss.Analysis(an => an
                            .TokenFilters(tf => tf
                                .Stop("english_stop", ts => ts
                                    .StopWords("_english_"))
                                .Stemmer("english_stemmer", st => st.Language("english"))
                                .EdgeNGram("edge_n_gram_analyzer",
                                    descriptor => descriptor.MaxGram(10).MinGram(2)))))
            );

            await client.Indices.CreateAsync("mgmt",
                index => index
                    .Map<Management>(x => x.AutoMap())
                    .Settings(ss =>
                        ss.Analysis(an => an
                            .TokenFilters(tf => tf
                                .Stop("english_stop", ts => ts
                                    .StopWords("_english_"))
                                .Stemmer("english_stemmer", st => st.Language("english"))
                                .EdgeNGram("edge_n_gram_analyzer",
                                    descriptor => descriptor.MaxGram(10).MinGram(2)))))
            );

            await client.BulkAsync(b => b.Index("property").IndexMany(properties));
            await client.BulkAsync(b => b.Index("mgmt").IndexMany(mgmts));

            Console.WriteLine("Done inserting!");
        }
    }
}