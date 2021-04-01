using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace SeleniumGrid_Testing
{
    public class ParallelSeleniumTesting
    {
        [Test]
        [Parallelizable]
        public void _01_Nakov_Title()
        {
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            string pageTile = _driver.Title;
            Assert.That(pageTile.Contains("Svetlin Nakov"));
            _driver.Quit();
        }

        [Test]
        public void _02_Nakov_OpenAboutPage()
        {
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            _driver.FindElementByCssSelector("body > section > div.about-author > div > a > img").Click();
            string pageTile = _driver.Title;
            Assert.AreEqual("https://nakov.com/about/", _driver.Url);
            _driver.Quit();
        }

        [Test]
        [Parallelizable]
        public void _03_Test_NakovCom_SearchQA()
        {
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            _driver.Navigate().GoToUrl("https://nakov.com/");
            _driver.Manage().Window.Size = new System.Drawing.Size(1406, 1040);
            _driver.FindElement(By.CssSelector("#sh > .smoothScroll")).Click();
            _driver.FindElement(By.Id("s")).Click();
            _driver.FindElement(By.Id("s")).SendKeys("QA");
            _driver.FindElement(By.Id("s")).SendKeys(Keys.Enter);
            _driver.FindElement(By.CssSelector(".entry-title")).Click();
            Thread.Sleep(1000);
            Assert.That(_driver.FindElement(By.CssSelector(".entry-title")).Text,
                Is.EqualTo("Search Results for – \"QA\""));
            _driver.Quit();
        }

        [Test]
        [Parallelizable]
        public void _04_Test_AddUrl()
        {
            // Add new Url
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
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
            _driver.Quit();
        }

        [Test]
        [Parallelizable]
        public void _05_Test_NakovSearch()
        {
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            var searchBox = _driver.FindElementByCssSelector("#s");
            searchBox.Click();
            searchBox.SendKeys("database");
            searchBox.SendKeys(Keys.Enter);
            var searchResultText = _driver.FindElementByCssSelector("h3.entry-title");
            var resultText = searchResultText.Text;
            Assert.AreEqual("Search Results for – \"database\"", resultText);
            _driver.Quit();
        }

        [Test]
        [Parallelizable]
        public void _06_Test_WikiSearch()
        {
            var options = new ChromeOptions();
            var _driver = new RemoteWebDriver(options) { Url = "https://nakov.com/" };
            _driver.Url = "https://en.wikipedia.org/";
            var searchBox = _driver.FindElementByXPath("//input[contains(@id,'searchInput')]");
            searchBox.Clear();
            searchBox.SendKeys("QA");
            searchBox.SendKeys(Keys.Enter);
            var headingQa = _driver.FindElementByXPath("//h1[contains(@id,'firstHeading')]");
            Assert.IsNotNull(headingQa);
            _driver.Quit();
        }
    }
}
