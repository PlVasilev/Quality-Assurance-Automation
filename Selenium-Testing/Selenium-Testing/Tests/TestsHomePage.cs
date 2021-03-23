using NUnit.Framework;
using Selenium_Testing.PageObjects;

namespace Selenium_Testing.Tests
{
    class TestsHomePage : BaseTest
    {
        [Test]
        public void Test_HomePage()
        {
            var homePage = new HomePage(driver);
            Assert.AreEqual("Students Registry", homePage.PageHeading);
            Assert.AreEqual("MVC Example", homePage.GetPageTitle);
            Assert.IsTrue(homePage.GetStudentsCount > -1);
        }

        [Test]
        public void Test_HomePage_Links()
        {
            var homePage = new HomePage(driver);
            homePage.Open();
            homePage.LinkHomePage.Click();
            Assert.IsTrue(new HomePage(driver).IsCurrentlyOpen());

            homePage.Open();
            homePage.LinkViewStudentsPage.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsCurrentlyOpen());

            homePage.Open();
            homePage.LinkAddStudentsPage.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsCurrentlyOpen());
        }
    }
}
