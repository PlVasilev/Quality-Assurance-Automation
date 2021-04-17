using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace ContactBook.AndroidTests
{
    public class Tests
    {
        private AndroidDriver<AndroidElement> _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions {PlatformName = "Android"};
            // To install app first
            options.AddAdditionalCapability("app",
                @"C:Path to app");
            // options.AddAdditionalCapability("appPackage", "vivino.web.app");
            // options.AddAdditionalCapability("appActivity", "com.sphinx_solution.activities.SplashActivity");
            _driver = new AndroidDriver<AndroidElement>(new Uri("http://[::1]:4723/wd/hub"), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public void Test01_SearchContact_ValidData()
        {
            var contactBookUrl = _driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            contactBookUrl.Clear();
            contactBookUrl.SendKeys("https://contactbook.nakov.repl.co/api");
            _driver.FindElementById("contactbook.androidclient:id/buttonConnect").Click();

            var searchField= _driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            searchField.Clear();
            searchField.SendKeys("steve");
            _driver.FindElementById("contactbook.androidclient:id/buttonSearch").Click();

            Thread.Sleep(2000);

            var searchResultsCount = _driver.FindElementById("contactbook.androidclient:id/textViewSearchResult").Text;
            var resultsNum = int.Parse(searchResultsCount.Split()[^1]);
            Assert.AreEqual(1,resultsNum);

            var firstName = _driver.FindElementById("contactbook.androidclient:id/textViewFirstName").Text;
            var lastName = _driver.FindElementById("contactbook.androidclient:id/textViewLastName").Text;

            Assert.AreEqual("Steve", firstName);
            Assert.AreEqual("Jobs", lastName);
        }
    }
}