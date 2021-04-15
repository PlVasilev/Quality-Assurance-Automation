using System.Collections.Generic;
using System.Net;
using ExPrep01.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace ExPrep01
{
    public class ApiTests
    {
        private const string BaseUrl = "https://shorturl.plvasilev.repl.co/api/";
        private const string NewUrlShortCode = "New101";
        private const string NewUrl = "https://NewSite101.Url";

        [OneTimeSetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test01_ListShortURLs()
        {
            var client = new RestClient($"{BaseUrl}urls");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var shortUrls = new JsonDeserializer().Deserialize<List<ShortUrlClass>>(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            foreach (var shortUrl in shortUrls)
            {
                string uRlType = shortUrl.Url.GetType().Name;
                Assert.AreEqual(uRlType, "String");

                string shortCodeType = shortUrl.ShortCode.GetType().Name;
                Assert.AreEqual(shortCodeType, "String");

                string shortUrlType = shortUrl.ShortUrl.GetType().Name;
                Assert.AreEqual(shortUrlType, "String");

                string dateCreated = shortUrl.DateCreated.GetType().Name;
                Assert.AreEqual(dateCreated, "DateTime");

                string visitsType = shortUrl.Visits.GetType().Name;
                Assert.AreEqual(visitsType, "Int32");
            }
        }

        [Test]
        public void Test02_FindURLByShortCode_ValidData()
        {
            var shortCode = "nak";
            var client = new RestClient($"{BaseUrl}urls/{shortCode}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var shortUrl = new JsonDeserializer().Deserialize<ShortUrlClass>(response);

            //Object Equality
            var expectedResult = new ShortUrlStr
            {
                Url = "https://nakov.com",
                ShortCode = "nak",
                ShortUrl = "http://shorturl.plvasilev.repl.co/go/nak",
                DateCreated = "2021-02-17T14:41:33Z",
                Visits = 160
            };
            var expectedStr = new JsonDeserializer().Serialize(shortUrl);
            var actualStr = new JsonDeserializer().Serialize(expectedResult);

            Assert.AreEqual(expectedStr, actualStr);


            Assert.AreEqual(shortUrl.Url, "https://nakov.com");
            Assert.AreEqual(shortUrl.ShortCode, "nak");
            Assert.AreEqual(shortUrl.ShortUrl, $"http://shorturl.plvasilev.repl.co/go/{shortCode}");
            Assert.IsTrue(shortUrl.DateCreated.ToString("O").StartsWith("2021-02-17T14:41:33.000"));
            Assert.AreEqual(shortUrl.Visits, 160);
        }

        [Test]
        public void Test03_FindURLByShortCode_InValidData()
        {
            var shortCode = "nakov";
            var client = new RestClient($"{BaseUrl}urls/{shortCode}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual(errMsg.ErrMsg, $"Short code not found: {shortCode}");
        }

        [Test]
        public void Test04_CreateNewShortURL_ValidData()
        {
            var client = new RestClient($"{BaseUrl}urls/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                url = NewUrl,
                shortCode = NewUrlShortCode,
            });
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var shortUrl = new JsonDeserializer().Deserialize<CreateUrlResponse>(response);

            Assert.AreEqual(NewUrl, shortUrl.Url.Url);
            Assert.AreEqual(NewUrlShortCode, shortUrl.Url.ShortCode);
        }



        [TestCase("https://NewSite101.Url", "New101", "Short code already exists!"
            , TestName = "Test05_CreateNewShortURL_InValidData_DuplicateShortCode")]
        [TestCase("", "New101", "URL cannot be empty!",
            TestName = "Test05_CreateNewShortURL_InValidData_EmptyUrl")]
        [TestCase("https://NewSite101.Url", "", "Short code cannot be empty!",
            TestName = "Test05_CreateNewShortURL_InValidData_EmptyShortCode")]
        [TestCase("NewSite.Url", "New101", "Invalid URL!",
            TestName = "Test05_CreateNewShortURL_InValidData_InvalidUrl")]
        [TestCase("https://NewSite101.Url", ":,.", "Short code holds invalid chars!",
            TestName = "Test05_CreateNewShortURL_InValidData_InvalidShortCodeChars")]
        public void Test05_CreateNewShortURL_InValidData_ExistentShortCode(string newUrl, string newShortCode,
            string expectedErrMsg)
        {
            var client = new RestClient($"{BaseUrl}urls/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                url = newUrl,
                shortCode = newShortCode,
            });
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual(expectedErrMsg, errMsg.ErrMsg);
        }

        [Test]
        public void Test06_VisitShortURL_ValidData()
        {
            var client = new RestClient($"{BaseUrl}urls/{NewUrlShortCode}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var shortUrl = new JsonDeserializer().Deserialize<ShortUrlClass>(response);
            var expectedVisits = shortUrl.Visits + 1;

            client = new RestClient($"{BaseUrl}urls/visit/{NewUrlShortCode}");
            request = new RestRequest(Method.POST);
            response = client.Execute(request);
            shortUrl = new JsonDeserializer().Deserialize<ShortUrlClass>(response);
            var visits = shortUrl.Visits;
            Assert.AreEqual(expectedVisits, visits);
        }

        [Test]
        public void Test06_VisitShortURL_InValidData()
        {
            var invalidShortCode = "NoSuchCode";
            var client = new RestClient($"{BaseUrl}urls/visit/{invalidShortCode}");
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual("Invalid short code!", errMsg.ErrMsg);
        }

        [Test]
        public void Test07_DeleteShortURL_InValidData()
        {
            var invalidShortCode = "NoSuchCode";
            var client = new RestClient($"{BaseUrl}urls/{invalidShortCode}");
            var request = new RestRequest(Method.DELETE);
            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual("Short code not found: NoSuchCode", errMsg.ErrMsg);
        }

        [Test]
        public void Test07_DeleteShortURL_ValidData()
        {
            var client = new RestClient($"{BaseUrl}urls/{NewUrlShortCode}");
            var request = new RestRequest(Method.DELETE);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            request = new RestRequest(Method.GET);
            response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual(errMsg.ErrMsg, $"Short code not found: {NewUrlShortCode}");
        }
    }
}