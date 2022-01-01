using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Nest;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ElasticApartment.SAM
{
    public class Functions
    {
        public Functions()
        {
        }


        public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var body = request.Body;
            var model = JsonConvert.DeserializeObject<QueryModel>(body);
            context.Logger.LogLine($"Post Request: SearchPhase: {model.SearchPhase}");
            var searchAsyncResponse = await SearchAsync(model);
            var searchAsyncResponseJsoSerializeObject = JsonConvert.SerializeObject(searchAsyncResponse);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = searchAsyncResponseJsoSerializeObject,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

        public async Task<IEnumerable<dynamic>> SearchAsync(QueryModel model)
        {
            var url = Environment.GetEnvironmentVariable("elasticUrl");
            var user = Environment.GetEnvironmentVariable("elasticUser");
            var pass = Environment.GetEnvironmentVariable("elasticPass");
           
            var settings = new ConnectionSettings(new Uri(url));
            settings.BasicAuthentication(user, pass);
            var elasticClient = new ElasticClient(settings);

            var limit = model.Limit.Equals(0) ? 25 : model.Limit;
            var multiMatchQuery = new MultiMatchQuery
            {
                Fuzziness = Fuzziness.EditDistance(3),
                Query = model.SearchPhase,
                Analyzer = "stop",
                AutoGenerateSynonymsPhraseQuery = true
            };

            var termsQuery = new TermsQuery();
            if (model.Market.Count > 0)
            {
                termsQuery.Field = "market.keyword";
                termsQuery.Terms = model.Market;
            }

            var searchDescriptor = new SearchDescriptor<dynamic>()
                .AllIndices()
                .Query(q => q.Bool(b => b
                    .Must(m => m.MultiMatch(_ => multiMatchQuery),
                        qt => qt.Terms(_ => termsQuery)
                    )));

            var response = await elasticClient.SearchAsync<dynamic>(searchDescriptor.Size(limit));

            return response.Hits.Select(r => r.Source);
        }
    }
}
