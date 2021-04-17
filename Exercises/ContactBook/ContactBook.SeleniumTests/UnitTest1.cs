using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace ContactBook.SeleniumTests
{
    public class Tests
    {
        protected RemoteWebDriver _driver;
        private const string BaseUrl = "https://contactbook.plvasilev.repl.co/";
        private IWebElement _contactLinkButton;
        private IWebElement _searchLinkButton;
        private IWebElement _createLinkButton;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            _driver = new ChromeDriver(options) {Url = BaseUrl};
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            _contactLinkButton = _driver.FindElementByXPath("//a[contains(.,'Contacts')]");
            _searchLinkButton = _driver.FindElementByXPath("//a[contains(.,'Search')]");
            _createLinkButton = _driver.FindElementByXPath("//a[contains(.,'Create')]");
        }

        [TearDown]
        public void ShutDown() => _driver.Quit();

        [Test]
        public void Test01_AssertFirstContactIsSteveJobs()
        {
            _contactLinkButton.Click();
            var firstContact = _driver.FindElementsByClassName("contact-entry")[0];
            var firstNameElement = firstContact.FindElement(By.ClassName("fname"));
            var firstName = firstNameElement.FindElement(By.CssSelector("td")).Text;
            var lastNameElement = firstContact.FindElement(By.ClassName("lname"));
            var lastName = lastNameElement.FindElement(By.CssSelector("td")).Text;

            Assert.AreEqual("Steve", firstName);
            Assert.AreEqual("Jobs", lastName);
        }

        [Test]
        public void Test02_SearchContact_ValidData()
        {
            _searchLinkButton.Click();
            var inputField = _driver.FindElementById("keyword");
            inputField.Clear();

            Thread.Sleep(1000);

            inputField.SendKeys("albert");

            _driver.FindElementById("search").Click();

            var searchedResults = _driver.FindElementById("searchResult").Text;
            var result = int.Parse(searchedResults.Split()[0]);

            Assert.AreEqual(1, result);

            var firstContact = _driver.FindElementsByClassName("contact-entry")[0];
            var firstNameElement = firstContact.FindElement(By.ClassName("fname"));
            var firstName = firstNameElement.FindElement(By.CssSelector("td")).Text;
            var lastNameElement = firstContact.FindElement(By.ClassName("lname"));
            var lastName = lastNameElement.FindElement(By.CssSelector("td")).Text;

            Assert.AreEqual("Albert", firstName);
            Assert.AreEqual("Einstein", lastName);
        }


        [Test]
        public void Test03_SearchContact_InValidData()
        {
            _searchLinkButton.Click();
            var inputField = _driver.FindElementById("keyword");
            inputField.Clear();

            Thread.Sleep(1000);

            inputField.SendKeys("invalid2635");

            _driver.FindElementById("search").Click();

            var searchedResults = _driver.FindElementById("searchResult").Text;

            Assert.AreEqual("No contacts found.", searchedResults);
        }

        [Test]
        public void Test04_CreateContact_InValidData()
        {
            _createLinkButton.Click();
            Thread.Sleep(1000);
            var inputFirstNameField = _driver.FindElementById("firstName");
            var inputLastNameField = _driver.FindElementById("lastName");
            var inputEmailNameField = _driver.FindElementById("email");

            inputFirstNameField.Clear();
            inputLastNameField.Clear();
            inputLastNameField.SendKeys("Curie");
            inputEmailNameField.Clear();
            inputEmailNameField.SendKeys("marie67@gmail.com");

            _driver.FindElementById("create").Click();
            Thread.Sleep(1000);

            var errMsg = _driver.FindElementByClassName("err").Text;
            Assert.AreEqual("Error: First name cannot be empty!", errMsg);

            inputFirstNameField = _driver.FindElementById("firstName");
            inputLastNameField = _driver.FindElementById("lastName");
            inputEmailNameField = _driver.FindElementById("email");

            inputFirstNameField.Clear();
            inputFirstNameField.SendKeys("Marie");
            inputLastNameField.Clear();
            inputEmailNameField.Clear();
            inputEmailNameField.SendKeys("marie67@gmail.com");

            _driver.FindElementById("create").Click();
            Thread.Sleep(1000);

            errMsg = _driver.FindElementByClassName("err").Text;
            Assert.AreEqual("Error: Last name cannot be empty!", errMsg);

            inputFirstNameField = _driver.FindElementById("firstName");
            inputLastNameField = _driver.FindElementById("lastName");
            inputEmailNameField = _driver.FindElementById("email");

            inputFirstNameField.Clear();
            inputFirstNameField.SendKeys("Marie");
            inputLastNameField.Clear();
            inputLastNameField.SendKeys("Curie");
            inputEmailNameField.Clear();

            _driver.FindElementById("create").Click();
            Thread.Sleep(1000);

            errMsg = _driver.FindElementByClassName("err").Text;
            Assert.AreEqual("Error: Invalid email!", errMsg);
        }

        [Test]
        public void Test05_CreateContact_ValidData()
        {
            _createLinkButton.Click();

            Thread.Sleep(1000);

            var inputFirstNameField = _driver.FindElementById("firstName");
            var inputLastNameField = _driver.FindElementById("lastName");
            var inputEmailNameField = _driver.FindElementById("email");

            inputFirstNameField.Clear();
            inputFirstNameField.SendKeys("Marie");
            inputLastNameField.Clear();
            inputLastNameField.SendKeys("Curie");
            inputEmailNameField.Clear();
            inputEmailNameField.SendKeys("marie67@gmail.com");

            _driver.FindElementById("create").Click();
            Thread.Sleep(2000);



            var firstContact = _driver.FindElementsByClassName("contact-entry")[^1];
            var firstNameElement = firstContact.FindElement(By.ClassName("fname"));
            var firstName = firstNameElement.FindElement(By.CssSelector("td")).Text;
            var lastNameElement = firstContact.FindElement(By.ClassName("lname"));
            var lastName = lastNameElement.FindElement(By.CssSelector("td")).Text;
            var emailElement = firstContact.FindElement(By.ClassName("email"));
            var email = emailElement.FindElement(By.CssSelector("td")).Text;

            Assert.AreEqual("Marie", firstName);
            Assert.AreEqual("Curie", lastName);
            Assert.AreEqual("marie67@gmail.com", email);
        }
    }
}