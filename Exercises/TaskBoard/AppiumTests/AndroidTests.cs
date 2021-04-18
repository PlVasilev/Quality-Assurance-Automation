using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace AppiumTests
{
    public class AndroidTests
    {
        const string AppiumUrl = "http://[::1]:4723/wd/hub/";
        const string AndroidAppPath = @"Path to file";
        const string TaskBoardApiUrl = "https://taskboard.nakov.repl.co/api";
        AndroidDriver<AndroidElement> _driver;

        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Android" };
            appiumOptions.AddAdditionalCapability("app", AndroidAppPath);
            _driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), appiumOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

      

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public void Test01_AssertFirstListedTaskTitle_ProjectSkeleton()
        {
            var expectedTitle = "Project skeleton";
            var textBoxUrl = _driver.FindElementById("taskboard.androidclient:id/editTextApiUrl");
            textBoxUrl.Clear();
            textBoxUrl.SendKeys(TaskBoardApiUrl);

            var buttonConnect = _driver.FindElementById("taskboard.androidclient:id/buttonConnect");
            buttonConnect.Click();

            var firstTask = _driver.FindElementsByClassName("android.widget.TableLayout")[0];
            var firstTaskTitle = firstTask.FindElementById("taskboard.androidclient:id/textViewTitle").Text;
            Assert.AreEqual(expectedTitle, firstTaskTitle);
        }

        [Test]
        public void Test02_CreateNewTaskAndSearchValidData_AssertTaskIsListed()
        {
            var textBoxUrl = _driver.FindElementById("taskboard.androidclient:id/editTextApiUrl");
            textBoxUrl.Clear();
            textBoxUrl.SendKeys(TaskBoardApiUrl);

            var buttonConnect = _driver.FindElementById("taskboard.androidclient:id/buttonConnect");
            buttonConnect.Click();

            var addTaskButton = _driver.FindElementById("taskboard.androidclient:id/buttonAdd");
            addTaskButton.Click();

            var titleFiled = _driver.FindElementById("taskboard.androidclient:id/editTextTitle");
            titleFiled.Clear();
            var title = "New Title" + Guid.NewGuid();
            titleFiled.SendKeys(title);

            var descriptionFiled = _driver.FindElementById("taskboard.androidclient:id/editTextDescription");
            descriptionFiled.Clear();
            descriptionFiled.SendKeys("New Description" + DateTime.Now.Ticks);

            var createButton = _driver.FindElementById("taskboard.androidclient:id/buttonCreate");
            createButton.Click();

            var expectedTitle = title;

            var searchFiled = _driver.FindElementById("taskboard.androidclient:id/editTextKeyword");
            searchFiled.Clear();
            searchFiled.SendKeys(title);

            var searchButton = _driver.FindElementById("taskboard.androidclient:id/buttonSearch");
            searchButton.Click();
            Thread.Sleep(1000);

            var firstSearchedResult = _driver.FindElementsById("taskboard.androidclient:id/recyclerViewTasks")[0];
            var firstSearchedResultTitle = firstSearchedResult.FindElementById("taskboard.androidclient:id/textViewTitle").Text;

            Assert.AreEqual(expectedTitle, firstSearchedResultTitle);
        }
    }
}