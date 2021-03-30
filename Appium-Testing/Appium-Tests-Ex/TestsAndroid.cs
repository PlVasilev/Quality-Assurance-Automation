using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace Appium_Tests_Ex
{
    public class TestsAndroid
    {
        private AndroidDriver<AndroidElement> _driver;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new AppiumOptions {PlatformName = "Android" };
            // To install app first
            // options.AddAdditionalCapability("app", @"path to App");

            //With Installed App
            options.AddAdditionalCapability("appPackage","vivino.web.app");
            options.AddAdditionalCapability("appActivity","com.sphinx_solution.activities.SplashActivity");
            _driver = new AndroidDriver<AndroidElement>(new Uri("http://[::1]:4723/wd/hub"), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
        }


        [Test]
        public void Test_VininoAppHighlightsAndFacts()
        {
            // Login
            var linkLogin = _driver.FindElementById("vivino.web.app:id/txthaveaccount");
            linkLogin.Click();

            var email = _driver.FindElementById("vivino.web.app:id/edtEmail");
            email.SendKeys("pesho@pesho.com");

            var password = _driver.FindElementById("vivino.web.app:id/edtPassword");
            password.SendKeys("1234");

            var logInButton = _driver.FindElementById("vivino.web.app:id/action_signin");
            logInButton.Click();

            var searchButton = _driver.FindElementById("vivino.web.app:id/wine_explorer_tab");
            searchButton.Click();

            //Search
            var searchBar = _driver.FindElementById("vivino.web.app:id/search_vivino");
            searchBar.Click();

            var searchText = _driver.FindElementById("vivino.web.app:id/editText_input");
            searchText.SendKeys("Katarzyna Reserve Red 2006");

            //Get first search result
            var winesList = _driver.FindElementById("vivino.web.app:id/listviewWineListActivity");
            var firstResult = winesList.FindElementByClassName("android.widget.FrameLayout");
            firstResult.Click();

            var wineName = _driver.FindElementById("vivino.web.app:id/wine_name");
            Assert.AreEqual("Reserve Red 2006", wineName.Text);

            var wineRating = _driver.FindElementById("vivino.web.app:id/rating");
            var rating = double.Parse(wineRating.Text);
            Assert.IsTrue(rating >= 4.00 && rating <= 5.00);

            //Get Summary
            var tabsSummary = _driver.FindElementById("vivino.web.app:id/tabs");
            var tabHighlights = tabsSummary.FindElementByXPath("//android.widget.TextView[1]");
            var tabFacts = tabsSummary.FindElementByXPath("//android.widget.TextView[2]");

            tabHighlights.Click();
            // var highLightsDescription = _driver.FindElementById("vivino.web.app:id/highlight_description");
            var highLightsDescription = _driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/highlight_description\"))");
            Assert.AreEqual("Among top 1% of all wines in the world", highLightsDescription.Text);

            tabFacts.Click();
            var factsTitleDescription = _driver.FindElementById("vivino.web.app:id/wine_fact_title");
            Assert.AreEqual("Grapes", factsTitleDescription.Text);

            var factsTextDescription = _driver.FindElementById("vivino.web.app:id/wine_fact_text");
            Assert.AreEqual("Cabernet Sauvignon,Merlot", factsTextDescription.Text);
        }
    }
}
