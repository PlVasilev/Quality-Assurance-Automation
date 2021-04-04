using DataDriven_Testing.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace DataDriven_Testing
{
    public class NUnitZippopotam
    {
        [TestCase("BG", "1000", "Sofija", TestName = "BG_1000 -> Sofija")]
        [TestCase("BG", "5000", "Veliko Turnovo", TestName = "BG_5000 -> Veliko Turnovo")]
        [TestCase("CA", "M5S", "Toronto", TestName = "CA_M5S -> Toronto")]
        [TestCase("GB", "B1", "Birmingham", TestName = "GB_B1 -> Birmingham")]
        [TestCase("DE", "01067", "Dresden", TestName = "DE_01067 -> Dresden")]
        public void Test_ByCountryAndZipCode_CorrectPlace(
            string countryCode, string zipCode, string expectedPlace)
        {
            var restClient = new RestClient("https://zippopotam.us/");
            var httpRequest = new RestRequest(countryCode + "/" + zipCode);

            var httpResponse = restClient.Execute(httpRequest);
            var location = new JsonDeserializer().Deserialize<Location>(httpResponse);
            var place = location.Places[0].PlaceName;
            Assert.That(place.Contains(expectedPlace));
        }
    }

}