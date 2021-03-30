using System;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Appium_Tests
{
    internal class AndroidTest
    {
        private AndroidDriver<AndroidElement> _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app",
                @"C:path to app");
            options.AddAdditionalCapability("platformName", "Android");
            _driver = new AndroidDriver<AndroidElement>(new Uri("http://[::1]:4723/wd/hub"), options);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public void Test_Summator_ValidData()
        {
            _driver.FindElementById("com.example.androidappsummator:id/editText1").Clear();
            _driver.FindElementById("com.example.androidappsummator:id/editText1").SendKeys("20");
            _driver.FindElementById("com.example.androidappsummator:id/editText2").Clear();
            _driver.FindElementById("com.example.androidappsummator:id/editText2").SendKeys("30");
            _driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum").Click();
            string sum = _driver.FindElementById("com.example.androidappsummator:id/editTextSum").Text;
            Assert.AreEqual("50", sum);
        }

        [Test]
        public void Test_Summator_InValidData()
        {
            _driver.FindElementById("com.example.androidappsummator:id/editText1").Clear();
            _driver.FindElementById("com.example.androidappsummator:id/editText1").SendKeys("");
            _driver.FindElementById("com.example.androidappsummator:id/editText2").Clear();
            _driver.FindElementById("com.example.androidappsummator:id/editText2").SendKeys("30");
            _driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum").Click();
            string sum = _driver.FindElementById("com.example.androidappsummator:id/editTextSum").Text;
            Assert.AreEqual("error", sum);
        }
    }
}
