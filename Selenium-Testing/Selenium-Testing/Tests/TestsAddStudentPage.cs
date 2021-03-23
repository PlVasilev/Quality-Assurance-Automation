using System;
using NUnit.Framework;
using Selenium_Testing.PageObjects;

namespace Selenium_Testing.Tests
{
    class TestsAddStudentPage : BaseTest
    {
        [Test]
        public void Test_AddStudentsPage()
        {
            var addStudentPage = new AddStudentPage(driver);
            Assert.AreEqual("Add Student", addStudentPage.GetPageTitle);
            Assert.AreEqual("Register New Student", addStudentPage.PageHeading);
            Assert.AreEqual("",addStudentPage.TextBoxEmail.Text);
            Assert.AreEqual("",addStudentPage.TextBoxName.Text);
            Assert.AreEqual("Add",addStudentPage.ButtonAdd.Text);

        }

        [Test]
        public void Test_AddStudentPage_Links()
        {
            var addStudentPage = new AddStudentPage(driver);

            addStudentPage.Open();
            addStudentPage.LinkViewStudentsPage.Click();
            Assert.IsTrue(new ViewStudentsPage(driver).IsCurrentlyOpen());

            addStudentPage.Open();
            addStudentPage.LinkHomePage.Click();
            Assert.IsTrue(new HomePage(driver).IsCurrentlyOpen());


            addStudentPage.Open();
            addStudentPage.LinkAddStudentsPage.Click();
            Assert.IsTrue(new AddStudentPage(driver).IsCurrentlyOpen());
        }

        [Test]
        public void Test_AddStudents_StudentsExist()
        {
            var addStudentPage = new AddStudentPage(driver);
            addStudentPage.CreateNewStudent(
                "peter" + DateTime.Now.Ticks, "peter@gmail.com");

            var viewStudentsPage = new ViewStudentsPage(driver);
            Assert.IsTrue(viewStudentsPage.IsCurrentlyOpen());

            var studentsList = viewStudentsPage.ListStudents.Text;
            Assert.IsTrue(studentsList.Contains("(peter@gmail.com)"));
            Assert.IsTrue(studentsList.Contains("peter"));
        }

        [Test]
        public void Test_AddStudents_AddValidStudent()
        {
            var addStudentPage = new AddStudentPage(driver);
            var name = "valid" + DateTime.Now.Ticks;
            var email = "valid@gmail.com";
            addStudentPage.CreateNewStudent(name, email);

            var viewStudentsPage = new ViewStudentsPage(driver);
            Assert.IsTrue(viewStudentsPage.IsCurrentlyOpen());

            var students = viewStudentsPage.GetRegisteredStudents;
            var newStudent = name + " (" + email + ")";
            Assert.Contains(newStudent, students);
        }

        [Test]
        public void Test_AddStudents_AddNotValidStudent()
        {
            var addStudentPage = new AddStudentPage(driver);
            var name = "";
            var email = "notValid@gmail.com";
            addStudentPage.CreateNewStudent(name, email);
            Assert.IsTrue(addStudentPage.IsCurrentlyOpen());
            Assert.IsTrue(addStudentPage.ErrorBoxStyle.Text.Contains("Cannot add student"));
        }
    }
}
