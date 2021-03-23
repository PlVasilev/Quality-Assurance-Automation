using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Selenium_Testing
{
    public class WikipediaSeleniumTest
    {
        private RemoteWebDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options) { Url = "https://nakov.com/" };

            // Fire Fox
            //  var ffService = FirefoxDriverService.CreateDefaultService();
            //  ffService.Host = "::1";
            // _driver = new FirefoxDriver(ffService) { Url = "https://nakov.com/" };
            // _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        }

        [OneTimeTearDown]
        public void Stop()
        {
            _driver.Quit();
        }

        [Test]
        public void _01_Nakov_Title()
        {
            string pageTile = _driver.Title;
            Assert.That(pageTile.Contains("Svetlin Nakov"));
        }

        [Test]
        public void _02_Nakov_OpenAboutPage()
        {
            // body > section > div.about-author > div > a > img
            _driver.FindElementByCssSelector("body > section > div.about-author > div > a > img").Click();
            string pageTile = _driver.Title;
            Assert.AreEqual("https://nakov.com/about/", _driver.Url);
        }

        [Test]
        public void _03_Test_NakovCom_SearchQA()
        {
            _driver.Navigate().GoToUrl("https://nakov.com/");
            _driver.Manage().Window.Size = new System.Drawing.Size(1406, 1040);
            _driver.FindElement(By.CssSelector("#sh > .smoothScroll")).Click();
            _driver.FindElement(By.Id("s")).Click();
            _driver.FindElement(By.Id("s")).SendKeys("QA");
            _driver.FindElement(By.Id("s")).SendKeys(Keys.Enter);
            _driver.FindElement(By.CssSelector(".entry-title")).Click();

            Assert.That(_driver.FindElement(By.CssSelector(".entry-title")).Text,
                Is.EqualTo("Search Results for – \"QA\""));
        }

        [Test]
        public void _04_Test_AddUrl()
        {
            // Add new Url
            _driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co/add-url");
            var textBoxUrl = _driver.FindElementById("url");
            var newUrl = "http://mynewurlpl" + DateTime.Now.Ticks + ".com";
            textBoxUrl.SendKeys(newUrl);

            var textBoxCode = _driver.FindElementByCssSelector("#code");
            var code = "code" + DateTime.Now.Ticks;
            textBoxCode.Clear();
            textBoxCode.SendKeys(code);
            var buttonSubmit = _driver.FindElementByXPath("//form/button[@type='submit']");
            buttonSubmit.Click();

            // Assert that new Url is added
            _driver.Navigate().GoToUrl("https://shorturl.nakov.repl.co/urls");
            var tableRows = _driver.FindElementsByCssSelector("table tr");
            var lastRow = tableRows.Last();
            var cells = lastRow.FindElements(By.CssSelector("td"));

            var firsCellText = cells[0].Text;
            Assert.AreEqual(newUrl, firsCellText);

            var secondCellText = cells[1].Text;
            Assert.AreEqual("http://shorturl.nakov.repl.co/go/" + code, secondCellText);
        }

        [Test]
        public void _05_Test_NakovSearch()
        {
            var searchBox = _driver.FindElementByCssSelector("#s");
            searchBox.Click();
            searchBox.SendKeys("database");
            searchBox.SendKeys(Keys.Enter);
            var searchResultText = _driver.FindElementByCssSelector("h3.entry-title");
            var resultText = searchResultText.Text;
            Assert.AreEqual("Search Results for – \"database\"", resultText);
        }

        [Test]
        public void _06_Test_WikiSearch()
        {
            _driver.Url = "https://en.wikipedia.org/";
            var searchBox = _driver.FindElementByXPath("//input[contains(@id,'searchInput')]");
            searchBox.Clear();
            searchBox.SendKeys("QA");
            searchBox.SendKeys(Keys.Enter);
            var headingQa = _driver.FindElementByXPath("//h1[contains(@id,'firstHeading')]");
            Assert.IsNotNull(headingQa);
        }
    }
}