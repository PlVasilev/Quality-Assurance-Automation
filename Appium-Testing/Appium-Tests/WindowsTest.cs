using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;

namespace Appium_Tests
{
    internal class WindowsTest
    {
        private WindowsDriver<WindowsElement> _driver;
        AppiumLocalService _localAppiumService;

        [OneTimeSetUp]
        public void Setup()
        {
            // npm install -g appium

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app",
                @"C:Path to App");
            options.AddAdditionalCapability("platformName", "Windows");
            //options.AddAdditionalCapability(MobileCapabilityType.DeviceName, "WindowsPC");
            _localAppiumService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _localAppiumService.Start();
            // _driver = new WindowsDriver<WindowsElement>(new Uri("http://[::1]:4723/wd/hub"), options);
            _driver = new WindowsDriver<WindowsElement>(_localAppiumService, options);
            
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
            _localAppiumService.Dispose();
        }

        [Test]
        public void Test_Summator_ValidData()
        {
            _driver.FindElementByAccessibilityId("textBoxFirstNum").Clear();
            _driver.FindElementByAccessibilityId("textBoxFirstNum").SendKeys("20");
            _driver.FindElementByAccessibilityId("textBoxSecondNum").Clear();
            _driver.FindElementByAccessibilityId("textBoxSecondNum").SendKeys("30");
            _driver.FindElementByAccessibilityId("buttonCalc").Click();
            string sum = _driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("50", sum);
        }

        [Test]
        public void Test_Summator_InValidData()
        {
            _driver.FindElementByAccessibilityId("textBoxFirstNum").Clear();
            _driver.FindElementByAccessibilityId("textBoxFirstNum").SendKeys("");
            _driver.FindElementByAccessibilityId("textBoxSecondNum").Clear();
            _driver.FindElementByAccessibilityId("textBoxSecondNum").SendKeys("30");
            _driver.FindElementByAccessibilityId("buttonCalc").Click();
            string sum = _driver.FindElementByAccessibilityId("textBoxSum").Text;
            Assert.AreEqual("error", sum);
        }
    }
}