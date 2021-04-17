using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace ExPrep01
{
    public class SeleniumTests
    {
        protected RemoteWebDriver _driver;
        private const string BaseUrl = "https://shorturl.plvasilev.repl.co";
        private const string ShortUrl = "http://shorturl.plvasilev.repl.co";
        private const string NewUrlShortCode = "New104";
        private const string NewUrl = "https://NewSite104.Url";

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options) { Url = BaseUrl };
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            _driver.Quit();
        }

        [Test]
        public void Test01_EnsureTableOFShortUrlsExists()
        {
            _driver.FindElementByXPath("//a[@href='/urls']").Click();
            var table = _driver.FindElementByCssSelector("table");
            Assert.That(table != null);
            var rows = table.FindElements(By.CssSelector("tr"));
            Assert.That(rows.Count > 0);
            var tableHeads = rows[0].FindElements(By.CssSelector("th"));
            Assert.That(tableHeads.Count == 4);
            Assert.That(tableHeads[0].Text == "Original URL");
            Assert.That(tableHeads[1].Text == "Short URL");
            Assert.That(tableHeads[2].Text == "Date Created");
            Assert.That(tableHeads[3].Text == "Visits");

            for (int i = 1; i < rows.Count; i++)
            {
                var tableData = rows[i].FindElements(By.CssSelector("td"));
                for (int j = 0; j < tableData.Count; j++)
                {
                    int number = 0;
                    Assert.That(tableData[0].Text.Contains("http"));
                    Assert.That(tableData[1].Text.Contains("http"));
                    var date = DateTime.TryParse(tableData[2].Text, out DateTime result);
                    Assert.That(date);
                    Assert.That(int.TryParse(tableData[3].Text, out number));
                }
            }
        }


        [TestCase("https://NewSite101.Url", "nak", "Short code already exists!"
            , TestName = "Test02_CreateNewShortURL_InValidData_DuplicateShortCode")]
        [TestCase("", "New101", "URL cannot be empty!",
            TestName = "Test02_CreateNewShortURL_InValidData_EmptyUrl")]
        [TestCase("https://NewSite101.Url", "", "Short code cannot be empty!",
            TestName = "Test02_CreateNewShortURL_InValidData_EmptyShortCode")]
        [TestCase("NewSite.Url", "New101", "Invalid URL!",
            TestName = "Test02_CreateNewShortURL_InValidData_InvalidUrl")]
        [TestCase("https://NewSite101.Url", ":,.", "Short code holds invalid chars!",
            TestName = "Test02_CreateNewShortURL_InValidData_InvalidShortCodeChars")]
        public void Test02_CreateNewShortUrl_InvalidData(string newUrl, string newShortCode,
            string expectedErrMsg)
        {
            _driver.FindElementByXPath("//a[@href='/add-url']").Click();
            Thread.Sleep(1000);
            var inputField = _driver.FindElementById("url");
            inputField.Clear();
            inputField.SendKeys($"{newUrl}");
            var inputCode = _driver.FindElementById("code");
            inputCode.Clear();
            inputCode.SendKeys($"{newShortCode}");
            _driver.FindElementByXPath("//button[@type='submit'][contains(.,'Create')]").Click();
            var errMsg = _driver.FindElementByClassName("err").Text;
            Assert.AreEqual(expectedErrMsg, errMsg);
        }

        [Test]
        public void Test02_CreateNewShortUrl_ValidData()
        {

            _driver.FindElementByXPath("//a[@href='/urls']").Click();
            Thread.Sleep(1000);
            var rowsCount = _driver.FindElements(By.CssSelector("tr")).Count;

            _driver.FindElementByXPath("//a[@href='/add-url']").Click();
            Thread.Sleep(1000);
            var inputField = _driver.FindElementById("url");
            inputField.Clear();
            inputField.SendKeys($"{NewUrl}");
            var inputCode = _driver.FindElementById("code");
            inputCode.Clear();
            inputCode.SendKeys($"{NewUrlShortCode}");
            _driver.FindElementByXPath("//button[@type='submit'][contains(.,'Create')]").Click();

            _driver.FindElementByXPath("//a[@href='/urls']").Click();
            Thread.Sleep(1000);
            var newRowsCount = _driver.FindElements(By.CssSelector("tr"));

            Assert.AreEqual(rowsCount + 1, newRowsCount.Count);

            var tableData = newRowsCount[^1].FindElements(By.CssSelector("td"));
            for (int j = 0; j < tableData.Count; j++)
            {
                Assert.That(tableData[0].Text == $"{NewUrl}");
                Assert.That(tableData[1].Text.EndsWith($"{NewUrlShortCode}"));
                var date = DateTime.TryParse(tableData[2].Text, out DateTime result);
                Assert.That(date);
                Assert.That(int.Parse(tableData[3].Text) == 0);
            }
        }

        [Test]
        public void Test03_VisitUrl_ExistingUrl()
        {

            _driver.FindElementByXPath("//a[@href='/urls']").Click();
            Thread.Sleep(1000);
            var rows = _driver.FindElements(By.CssSelector("tr"));
            int visitsCount = 0;
            for (int i = 1; i < rows.Count; i++)
            {
                var tableData = rows[i].FindElements(By.CssSelector("td"));
                var shortUlr = tableData[1].Text;
                var searchedShortUrl = $"{ShortUrl}/go/{NewUrlShortCode}";
                if (shortUlr == searchedShortUrl)
                {
                    visitsCount = int.Parse(tableData[3].Text);
                    tableData[1].Click();
                    Thread.Sleep(2000);
                    break;
                }
            }

            rows = _driver.FindElements(By.CssSelector("tr"));
            int newVisitsCount = 0;
            for (int i = 1; i < rows.Count; i++)
            {
                var tableData = rows[i].FindElements(By.CssSelector("td"));
                if (tableData[1].Text == $"{ShortUrl}/go/{NewUrlShortCode}")
                {
                    newVisitsCount = int.Parse(tableData[3].Text);
                    tableData[1].Click();
                    Thread.Sleep(2000);
                    break;
                }
            }
            Assert.AreEqual(visitsCount + 1, newVisitsCount);
        }

        [Test]
        public void Test03_VisitUrl_NonExistingUrl()
        {

            _driver.FindElementByXPath("//a[@href='/urls']").Click();
            Thread.Sleep(1000);
            _driver.Navigate().GoToUrl($"{ShortUrl}/go/{NewUrlShortCode}aaaa");
            var errMsg = _driver.FindElementByClassName("err").Text;
            Assert.AreEqual("Invalid short code!", errMsg);
        }
    }
}
