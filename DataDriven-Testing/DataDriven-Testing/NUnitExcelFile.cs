using System.Collections.Generic;
using DataDriven_Testing.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp;
using RestSharp.Serialization.Json;
using SpreadsheetLight;

namespace DataDriven_Testing
{
    public class NUnitExcelFile
    {
        [TestCaseSource("LoadTestDataFromExcel")]
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
        static IEnumerable<TestCaseData> LoadTestDataFromExcel()
        {
            using var sheet = new SLDocument("../../../Locations.xlsx");
            int rowsCount = sheet.GetWorksheetStatistics().EndRowIndex;
            for (int row = 2; row <= rowsCount; row++)
            {
                var countryCode = sheet.GetCellValueAsString(row, 1);
                var zipCode = sheet.GetCellValueAsString(row, 2);
                var expectedPlace = sheet.GetCellValueAsString(row, 3);
                yield return new TestCaseData(countryCode, zipCode, expectedPlace)
                    .SetName($"Test_{countryCode}_{zipCode}->{expectedPlace}");
            }

            // yield return new TestCaseData("BG", "1000", "Sofija");
            // yield return new TestCaseData("BG", "5000", "Veliko Turnovo");
            // yield return new TestCaseData("CA", "M5S" , "Toronto");
            // yield return new TestCaseData("GB", "B1"  , "Birmingham" );
            // yield return new TestCaseData("DE", "01067", "Dresden");
        }
    }
}


