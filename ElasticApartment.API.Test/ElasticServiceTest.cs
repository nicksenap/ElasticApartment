using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ElasticApartment.API.Services;
using ElasticApartment.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using Xunit;

namespace ElasticApartment.API.Test
{
    public class ElasticServiceTest
    {
        [Fact]
        public async void search_async_should_return_two_hits()
        {
            // Arrange
            var elasticClient = SetupElasticClient();
            var elasticService = new ElasticService(elasticClient, new Mock<ILogger<ElasticService>>().Object);

            // Act
            var searchResult = await elasticService.SearchAsync(new QueryModel
            {
                Limit = 10,
                Market = new List<string> { "Austin" },
                SearchPhase = "Timber"
            });

            // Assert
            Assert.Equal(2, searchResult.Count());
        }

        private static IElasticClient SetupElasticClient()
        {
            var mockSearchResponse = MockSearchResponse();

            var elasticClient = new Mock<IElasticClient>();
            elasticClient.Setup(e => e
                    .SearchAsync<dynamic>(It.IsAny<ISearchRequest>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockSearchResponse.Object);

            return elasticClient.Object;
        }

        private static Mock<ISearchResponse<Property>> MockSearchResponse()
        {
            var people = new List<Property>
            {
                new()
                {
                    PropertyId = 85631,
                    Name = "Riatta Ranch",
                    FormerName = "",
                    StreetAddress = "1111 Musken",
                    City = "Abilene",
                    Market = "Abilene",
                    State = "TX",
                    Lat = 3.245346100000000e+001,
                    Lng = -9.970416200000000e+001
                },
                new()
                {
                    PropertyId = 85633,
                    Name = "Timber Ridge",
                    FormerName = "",
                    StreetAddress = "3602 Rolling Green Drive",
                    City = "Abilene",
                    Market = "Abilene",
                    State = "TX",
                    Lat = 3.245346100000000e+001,
                    Lng = -9.970416200000000e+001
                }
            };

            var hits = new List<IHit<Property>>
            {
                new Mock<IHit<Property>>().Object,
                new Mock<IHit<Property>>().Object
            };

            var mockSearchResponse = new Mock<ISearchResponse<Property>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(people);
            mockSearchResponse.Setup(x => x.Hits).Returns(hits);
            
            return mockSearchResponse;
        }
    }
}