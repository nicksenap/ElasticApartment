# ElasticApartment API
ElasticApartment is a Web API built with .NET 5 and ElasticSearch
## To run the API
Start project `ElasticApartment.API` from visual studio and test the Post endpoint with swagger or Postman/cURL:
```bash
curl --location --request POST 'https://localhost:5001/api/Search' \
--header 'Content-Type: application/json' \
--data-raw '{
    "searchPhase": "Jefferson Park",
    "market": ["Austin", "DFW"],
    "limit": 5
}'
```
## To run json uploader for ElasticSearch
Start project `ElasticApartment.Insert` from visual studio, make sure to have the correct path to json files in `appsetting.json`
## The serverless alternative
The project `ElasticApartment.SAM` is the serverless alternative for this api, build using [AWS SAM](https://aws.amazon.com/serverless/sam/) framework,
this API (or the Post method) we been using is presented a Lambda function with a API Gateway in front of it to recieve request.
## To deploy the serverless api
Go to `\ElasticApartment\ElasticApartment.SAM` folder and run `dotnet lambda deploy-serverless`,
make sure to have correct aws credential profile and region in the `aws-lambda-tools-defaults.json` file
## To try the serverless api
The serverless api is deployed to AWS and can be reach by:
```bash
curl --location --request POST 'https://wtwxpuj1tk.execute-api.eu-north-1.amazonaws.com/Prod/' \
--header 'Content-Type: application/json' \
--data-raw '{
    "searchPhase": "Jefferson Park",
    "market": ["Austin", "DFW"],
    "limit": 5
}'
```
