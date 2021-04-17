using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace ContactBook.AndroidTests
{
    public class WindowsAppTestsContactBook
    {
        const string AppiumUrl = "http://[::1]:4723/wd/hub/";
        const string AndroidAppPath = @"C:\Work\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";
        const string ContactBookApiUrl = "https://contactbook.nakov.repl.co/api";
        WindowsDriver<WindowsElement> driver;

        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Windows" };
            appiumOptions.AddAdditionalCapability("app", AndroidAppPath);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), appiumOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void Test_Search_Albert()
        {
            var textBoxApiUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            textBoxApiUrl.Clear();
            textBoxApiUrl.SendKeys(ContactBookApiUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            Thread.Sleep(500);

            string windowName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowName);

            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearch");
            textBoxSearch.Clear();
            textBoxSearch.SendKeys("albert");

            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            wait.Until(d => {
                var labelResult = driver.FindElementByAccessibilityId("labelResult");
                return labelResult.Text.StartsWith("Contacts found:");
            });

            var labelResult = driver.FindElementByAccessibilityId("labelResult");
            Assert.That(labelResult.Text.StartsWith("Contacts found:"));

            var tableContacts = driver.FindElementByAccessibilityId("dataGridViewContacts");
            var cellFirstName = tableContacts.FindElementByXPath(
                "//Edit[@Name='FirstName Row 0, Not sorted.']");
            Assert.That(cellFirstName.Text, Is.EqualTo("Albert"));

            var cellLastName = tableContacts.FindElementByXPath(
                "//Edit[@Name='LastName Row 0, Not sorted.']");
            Assert.That(cellLastName.Text, Is.EqualTo("Einstein"));
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}