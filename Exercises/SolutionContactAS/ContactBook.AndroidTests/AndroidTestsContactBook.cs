using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace ContactBook.AndroidTests
{
    public class AndroidTestsContactBook
    {
        const string AppiumUrl = "http://[::1]:4723/wd/hub/";
        const string AndroidAppPath = @"C:\Work\contactbook-androidclient.apk";
        const string ContactBookApiUrl = "https://contactbook.nakov.repl.co/api";
        AndroidDriver<AndroidElement> driver;

        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Android" };
            appiumOptions.AddAdditionalCapability("app", AndroidAppPath);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), appiumOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Test_Search_Albert()
        {
            var textBoxUrl = driver.FindElementById("contactbook.androidclient:id/editTextApiUrl");
            textBoxUrl.Clear();
            textBoxUrl.SendKeys(ContactBookApiUrl);

            var buttonConnect = driver.FindElementById("contactbook.androidclient:id/buttonConnect");
            buttonConnect.Click();

            var textBoxKeyword = driver.FindElementById("contactbook.androidclient:id/editTextKeyword");
            textBoxKeyword.Clear();
            textBoxKeyword.SendKeys("albert");

            var buttonSearch = driver.FindElementById("contactbook.androidclient:id/buttonSearch");
            buttonSearch.Click();

            var cellFirstName = driver.FindElementById("contactbook.androidclient:id/textViewFirstName");
            Assert.That(cellFirstName.Text, Is.EqualTo("Albert"));

            var cellLastName = driver.FindElementById("contactbook.androidclient:id/textViewLastName");
            Assert.That(cellLastName.Text, Is.EqualTo("Einstein"));
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}