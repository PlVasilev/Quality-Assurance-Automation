using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace ExPrep01
{
    class AppiumTests
    {
        private AndroidDriver<AndroidElement> _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions {PlatformName = "Android"};
            // To install app first
            options.AddAdditionalCapability("app",
                @"C:\Users\plamen\Desktop\SodtUni Courses\QA\Automation\Exercises\Exercises\ExPrep01\com.android.example.github.apk");
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
        public void Test_GitHubRepoBrowser()
        {
            var searchField = _driver.FindElementById("com.android.example.github:id/input");
            searchField.Clear();
            searchField.Click();
            searchField.SendKeys("Selenium");
            _driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);
            var searchResults = _driver.FindElementsById("com.android.example.github:id/name");
            var searchedResult = searchResults.FirstOrDefault(x => x.Text == "SeleniumHQ/selenium");
            searchedResult?.Click();
            var personsResults = _driver.FindElementsById("com.android.example.github:id/textView");
            var personsResult = personsResults.FirstOrDefault(x => x.Text == "barancev");
            personsResult?.Click();
            var personName = _driver.FindElementById("com.android.example.github:id/name").Text;
            Assert.AreEqual("Alexei Barantsev",personName);
        }
    }
}
