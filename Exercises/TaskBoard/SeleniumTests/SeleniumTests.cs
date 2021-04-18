using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    public class SeleniumTests
    {
        protected RemoteWebDriver _driver;
        private const string BaseUrl = "https://taskboard.plvasilev.repl.co";

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            // options.AddArgument("--headless");
            _driver = new ChromeDriver(options) { Url = BaseUrl };
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        [OneTimeTearDown]
        public void ShutDown() => _driver.Quit();

        [Test]
        public void Test01_ListTasksCheckForBoardNameDone_FindTitleProjectSelection()
        {
            var expectedFirstTaskTitle = "Project skeleton";
            _driver.Navigate().GoToUrl(BaseUrl + "/");
            var iconTaskBoard = _driver.FindElementByPartialLinkText("Task Board");
            iconTaskBoard.Click();
            var tasks = _driver.FindElementsByClassName("task");
            var doneTasks = tasks.FirstOrDefault(x => x.FindElement(By.CssSelector("h1")).Text == "Done");
            var firstDoneTask = doneTasks.FindElements(By.CssSelector("table"))[0];
            var firstDoneTaskTitle = firstDoneTask.FindElement(By.CssSelector("tr.title td")).Text;

            // Works too
            // var firstDoneTaskTitle = _driver.FindElementByCssSelector("table#task1 tr.title td").Text;
            Assert.AreEqual(expectedFirstTaskTitle, firstDoneTaskTitle);
        }

        [Test]
        public void Test02_FindTaskByKeyWordHome_FindTitleHomePage()
        {
            var expectedFirstTaskByKeyWordTitle = "Home page";
            var keyword = "home";
            _driver.Navigate().GoToUrl(BaseUrl + "/");
            var iconSearchTasks = _driver.FindElementByPartialLinkText("Search Tasks");
            iconSearchTasks.Click();
            Thread.Sleep(1000);
            var keywordField = _driver.FindElementById("keyword");
            keywordField.SendKeys(keyword);
            var searchButton = _driver.FindElementById("search");
            searchButton.Click();

            var firstTaskFound = _driver.FindElementsByClassName("task")[0];
            var firstTaskByKeywordTitle = firstTaskFound.FindElement(By.CssSelector("tr.title td")).Text;

            Assert.AreEqual(expectedFirstTaskByKeyWordTitle, firstTaskByKeywordTitle);
        }

        [Test]
        public void Test03_FindTaskByKeyWordHome_InvalidKeyWord()
        {
            var keyword = Guid.NewGuid().ToString();
            var expectedCount = 0;
            _driver.Navigate().GoToUrl(BaseUrl + "/");
            var iconSearchTasks = _driver.FindElementByPartialLinkText("Search Tasks");
            iconSearchTasks.Click();
            Thread.Sleep(1000);
            var keywordField = _driver.FindElementById("keyword");
            keywordField.SendKeys(keyword);
            var searchButton = _driver.FindElementById("search");
            searchButton.Click();

            var tasksFound = _driver.FindElementsByClassName("task");

            Assert.AreEqual(expectedCount, tasksFound.Count);
        }

        [Test]
        public void Test04_CreateTask_InvalidData()
        {
            var expectedErrorMessage = "Error: Title cannot be empty!";
            _driver.Navigate().GoToUrl(BaseUrl + "/");
            var iconCreateTasks = _driver.FindElementByPartialLinkText("New Task");
            iconCreateTasks.Click();
            Thread.Sleep(1000);

            var titleField = _driver.FindElementById("title");
            titleField.Clear();
            var createButton = _driver.FindElementById("create");
            createButton.Click();
            Thread.Sleep(1000);

            var errorMessage = _driver.FindElementByClassName("err").Text;

            Assert.AreEqual(expectedErrorMessage, errorMessage);
        }

        [TestCase("Open", TestName = "Test05_CreateTaskOpenBoard_ValidData")]
        [TestCase("In Progress", TestName = "Test05_CreateTaskInProgressBoard_NoTitle")]
        [TestCase("Done", TestName = "Test05_CreateTaskDoneBoard_NoTitle")]
        public void Test05_CreateTask_ValidData(string boardName)
        {
            var title = "New Title" + DateTime.Now.Ticks;
            _driver.Navigate().GoToUrl(BaseUrl + "/");
            var iconCreateTasks = _driver.FindElementByPartialLinkText("New Task");
            iconCreateTasks.Click();
            Thread.Sleep(1000);

            var titleField = _driver.FindElementById("title");
            titleField.Clear();
            titleField.SendKeys(title);

            var descriptionField = _driver.FindElementById("description");
            descriptionField.Clear();
            descriptionField.SendKeys("SeleniumTest " + boardName);

            var boardSelectFiled = _driver.FindElementById("boardName");
            var selectElement = new SelectElement(boardSelectFiled);
            selectElement.SelectByValue(boardName);

            var createButton = _driver.FindElementById("create");
            createButton.Click();

            Thread.Sleep(1000);

            var allTasks = _driver.FindElementsByClassName("task");
            var tasksByBoardName = allTasks.FirstOrDefault(x => x.FindElement(By.CssSelector("h1")).Text == boardName);
            var createdTasksByBoardName = tasksByBoardName.FindElements(By.CssSelector("table")).ToList();
            var lastCreatedTask = createdTasksByBoardName.Last();
            var lastCreatedTaskTitle = lastCreatedTask.FindElement(By.CssSelector("tr.title td")).Text;

            Assert.AreEqual(title, lastCreatedTaskTitle);
        }
    }
}