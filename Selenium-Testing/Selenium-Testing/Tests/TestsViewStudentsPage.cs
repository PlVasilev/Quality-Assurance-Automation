using System;
using NUnit.Framework;
using Selenium_Testing.PageObjects;

namespace Selenium_Testing.Tests
{
    class TestsViewStudentsPage : BaseTest
    {
        [Test]
        public void Test_ViewStudentsPage()
        {
            var studentsPage = new ViewStudentsPage(driver);
            Assert.AreEqual("Students", studentsPage.GetPageTitle);
            Assert.AreEqual("Registered Students", studentsPage.PageHeading);
        }


        [Test]
        public void Test_StudentPage_Links()
        {
            var viewStudentPage = new ViewStudentsPage(driver);

            viewStudentPage.Open();
            viewStudentPage.LinkAddStudentsPage.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsCurrentlyOpen());

            viewStudentPage.Open();
            viewStudentPage.LinkViewStudentsPage.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsCurrentlyOpen());

            viewStudentPage.Open();
            viewStudentPage.LinkHomePage.Click();
            Assert.IsTrue(new HomePage(driver).IsCurrentlyOpen());
        }

        [Test]
        public void Test_ViewStudents_StudentsExist()
        {
            var studentsPage = new ViewStudentsPage(driver);
            var students = studentsPage.ListStudents.Text;
            Assert.IsNotNull(students);
        }

        [Test]
        public void Test_ViewStudents_EachStudentIsValid()
        {
            var studentsPage = new ViewStudentsPage(driver);
            var students = studentsPage.GetRegisteredStudents;
            foreach (var student in students)
            {
                Assert.IsTrue(student.IndexOf("(", StringComparison.Ordinal) > 0);
                Assert.IsTrue(student.IndexOf(")", StringComparison.Ordinal) == student.Length-1);
            }
        }
    }
}
