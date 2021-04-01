using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace SeleniumGrid_Testing
{
    [TestFixture(typeof(FirefoxOptions))]
    [TestFixture(typeof(ChromeOptions))]
    public class MultiSeleniumTest<TOptions> where TOptions : DriverOptions, new()
    {
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new TOptions();
            _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
        }

        [OneTimeTearDown]
        public void Stop() =>_driver.Quit();
        

        [Test]
        public void _01_Nakov_Title()
        {
            string pageTile = _driver.Title;
            Assert.That(pageTile.Contains("Svetlin Nakov"));
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
            Thread.Sleep(1000);
            _driver.FindElement(By.CssSelector(".entry-title")).Click();
            Assert.That(_driver.FindElement(By.CssSelector(".entry-title")).Text,
                Is.EqualTo("Search Results for – \"QA\""));
        }
    }
}