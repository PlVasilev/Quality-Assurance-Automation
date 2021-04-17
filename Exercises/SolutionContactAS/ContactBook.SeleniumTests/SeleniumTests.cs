using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Linq;
using System.Threading;

namespace ContactBook.SeleniumTests
{
    public class SeleniumTests
    {
        RemoteWebDriver driver;
        const string AppBaseUrl = "https://contactbook.nakov.repl.co";

        [OneTimeSetUp]
        public void Setup()
        {
            //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            //service.Host = "::1";
            //this.driver = new FirefoxDriver(service);

            this.driver = new ChromeDriver();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [Test]
        public void Test_ListContacts_CheckForSteveJobs()
        {
            // Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/");
            var iconViewContacts = driver.FindElementByPartialLinkText("View contacts");

            // Act
            iconViewContacts.Click();

            // Assert
            var cellFirstName = driver.FindElementByCssSelector("table tr.fname td");
            Assert.That(cellFirstName.Text, Is.EqualTo("Steve"));
            var cellLastName = driver.FindElementByCssSelector("table tr.lname td");
            Assert.That(cellLastName.Text, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_ListContacts_SearchForAlbert()
        {
            // Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/");
            var iconSearch = driver.FindElementByPartialLinkText("Search contacts");
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxSearch = driver.FindElementByCssSelector("input#keyword");
            textBoxSearch.SendKeys("albert");

            var buttonSearch = driver.FindElementByCssSelector("button#search");
            buttonSearch.Click();

            // Assert
            var pageHeading = driver.FindElementByCssSelector("main > h1");
            Assert.That(pageHeading.Text, Is.EqualTo("Contacts Matching Keyword \"albert\""));

            var cellFirstName = driver.FindElementByCssSelector("table tr.fname td");
            Assert.That(cellFirstName.Text, Is.EqualTo("Albert"));
            var cellLastName = driver.FindElementByCssSelector("table tr.lname td");
            Assert.That(cellLastName.Text, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_SearchContacts_InvalidKeyword()
        {
            // Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/");
            var iconSearch = driver.FindElementByPartialLinkText("Search contacts");
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxSearch = driver.FindElementByCssSelector("input#keyword");
            textBoxSearch.SendKeys("invalid67433646321");

            var buttonSearch = driver.FindElementByCssSelector("button#search");
            buttonSearch.Click();

            // Assert
            var searchResults = driver.FindElementByCssSelector("div#searchResult");
            Assert.That(searchResults.Text, Is.EqualTo("No contacts found."));
        }

        [Test]
        public void Test_CreateContact_InvalidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/");
            var iconSearch = driver.FindElementByPartialLinkText("Create new contact");
            iconSearch.Click();

            Thread.Sleep(500);

            // Act
            var textBoxFirstName = driver.FindElementByCssSelector("input#firstName");
            textBoxFirstName.SendKeys("Pesho");

            var buttonCreate = driver.FindElementByCssSelector("button#create");
            buttonCreate.Click();

            // Assert
            var errorMsg = driver.FindElementByCssSelector("div.err");
            Assert.That(errorMsg.Text, Is.EqualTo("Error: Last name cannot be empty!"));
        }


        [Test]
        public void Test_CreateContact_ValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/");
            var iconSearch = driver.FindElementByPartialLinkText("Create new contact");
            iconSearch.Click();
            Thread.Sleep(500);
            string firstName = "First Name " + DateTime.Now.Ticks;
            string lastName = "Last Name " + DateTime.Now.Ticks;
            string email = "email" + DateTime.Now.Ticks + "@gmail.com";

            // Act
            var textBoxFirstName = driver.FindElementByCssSelector("input#firstName");
            textBoxFirstName.SendKeys(firstName);

            var textBoxLastName = driver.FindElementByCssSelector("input#lastName");
            textBoxLastName.SendKeys(lastName);

            var textBoxEmail = driver.FindElementByCssSelector("input#email");
            textBoxEmail.SendKeys(email);

            var buttonCreate = driver.FindElementByCssSelector("button#create");
            buttonCreate.Click();

            // Assert
            var contactEntries = driver.FindElementsByCssSelector("table.contact-entry");
            var lastContact = contactEntries.Last();

            var cellFirstName = lastContact.FindElement(By.CssSelector("tr.fname > td"));
            Assert.That(cellFirstName.Text, Is.EqualTo(firstName));

            var cellLastName = lastContact.FindElement(By.CssSelector("tr.lname > td"));
            Assert.That(cellLastName.Text, Is.EqualTo(lastName));

            var cellEmail = lastContact.FindElement(By.CssSelector("tr.email > td"));
            Assert.That(cellEmail.Text, Is.EqualTo(email));
        }

        [OneTimeTearDown]
        public void Shutdown()
        {
            this.driver.Quit();
        }
    }
}