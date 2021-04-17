using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace ContactBook.DesktopTests
{
    public class Tests
    {
        private WindowsDriver<WindowsElement> _driver;
        AppiumLocalService _localAppiumService;

        [OneTimeSetUp]
        public void Setup()
        {
            // npm install -g appium

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app",
                @"C:Path to app");
            options.AddAdditionalCapability("platformName", "Windows");
            _localAppiumService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _localAppiumService.Start();
            _driver = new WindowsDriver<WindowsElement>(_localAppiumService, options);

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
            _localAppiumService.Dispose();
        }

        [Test]
        public void Test01_SearchContact_ValidData()
        {
            //  "[@AutomationId=\"buttonConnect\"]";
            // "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@Name=\"Search Contacts\"][@AutomationId=\"SearchContactsForm\"]/Edit[@AutomationId=\"textBoxSearch\"]"
            // "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@Name=\"Search Contacts\"][@AutomationId=\"SearchContactsForm\"]/Button[@Name=\"Search\"][@AutomationId=\"buttonSearch\"]"
            // "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@Name=\"Search Contacts\"][@AutomationId=\"SearchContactsForm\"]/Table[@Name=\"Enter a keyword for searching:\"][@AutomationId=\"dataGridViewContacts\"]/Custom[@Name=\"Row 0\"]/Edit[@Name=\"FirstName Row 0, Not sorted.\"]"
            // "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@Name=\"Search Contacts\"][@AutomationId=\"SearchContactsForm\"]/Table[@Name=\"Enter a keyword for searching:\"][@AutomationId=\"dataGridViewContacts\"]/Custom[@Name=\"Row 0\"]/Edit[@Name=\"LastName Row 0, Not sorted.\"]"
            _driver.FindElementByAccessibilityId("buttonConnect").Click();
            string windowName = _driver.WindowHandles[0];
            _driver.SwitchTo().Window(windowName);

            _driver.FindElementByAccessibilityId("textBoxSearch").Clear();
            _driver.FindElementByAccessibilityId("textBoxSearch").SendKeys("steve");
            Thread.Sleep(2000);
            _driver.FindElementByAccessibilityId("buttonSearch").Click();
            Thread.Sleep(2000);
            var firstName = _driver.FindElementByXPath("/Window/Table/Custom/Edit[@Name=\"FirstName Row 0, Not sorted.\"]").Text;
            var lastName = _driver.FindElementByXPath("/Window/Table/Custom/Edit[@Name=\"LastName Row 0, Not sorted.\"]").Text;


            Assert.AreEqual("Steve", firstName);
            Assert.AreEqual("Jobs", lastName);
        }
    }
}