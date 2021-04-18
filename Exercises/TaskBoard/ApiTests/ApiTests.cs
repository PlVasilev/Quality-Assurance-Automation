using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApiTests.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace ApiTests
{
    public class ApiTests
    {
        const string ApiBaseUrl = "https://taskboard.plvasilev.repl.co/api";
        RestClient _client;

        [SetUp]
        public void Setup()
        {
            this._client = new RestClient(ApiBaseUrl);
        }

        [Test]
        public void Test01_ListTasksCheckForBoardNameDone_FindTitleProjectSelection()
        {
            // Arrange
            var searchedBoardName = "Done";
            var expectedTaskTitle = "Project skeleton";
            var request = new RestRequest("/tasks", Method.GET);

            // Act
            var response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tasks = new JsonDeserializer().Deserialize<List<Task>>(response);
            Assert.That(tasks.Count, Is.GreaterThan(0));


            foreach (var task in tasks)
            {
                string id = task.id.GetType().Name;
                Assert.AreEqual(id, "Int32");

                string title = task.title.GetType().Name;
                Assert.AreEqual(title, "String");

                string description = task.description.GetType().Name;
                Assert.AreEqual(description, "String");

                string dateCreated = task.dateCreated.GetType().Name;
                Assert.AreEqual(dateCreated, "String");

                string dateModified = task.dateModified.GetType().Name;
                Assert.AreEqual(dateModified, "String");

                string boardId = task.board.id.GetType().Name;
                Assert.AreEqual(boardId, "Int32");

                string boardName = task.board.name.GetType().Name;
                Assert.AreEqual(boardName, "String");
            }

            var firstBoardDoneTasks = tasks.First(x => x.board.name == searchedBoardName);
            Assert.AreEqual(expectedTaskTitle, firstBoardDoneTasks.title);
        }

        [Test]
        public void Test02_FindTaskByKeyWordHome_FindTitleHomePage()
        {
            // Arrange
            var searchedKeyword = "home";
            var expectedTaskTitle = "Home page";
            var request = new RestRequest("/tasks/search/{keyword}", Method.GET);
            request.AddUrlSegment("keyword", searchedKeyword);

            // Act
            var response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tasks = new JsonDeserializer().Deserialize<List<Task>>(response);
            Assert.That(tasks.Count, Is.GreaterThan(0));
            var firstTask = tasks[0];
            Assert.AreEqual(expectedTaskTitle, firstTask.title);
        }

        [Test]
        public void Test03_FindTaskKeyByInvalidKeyWord_AssertEmptyResult()
        {
            // Arrange
            var searchedKeyword = Guid.NewGuid().ToString();
            var request = new RestRequest("/tasks/search/{keyword}", Method.GET);
            request.AddUrlSegment("keyword", searchedKeyword);

            // Act
            var response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tasks = new JsonDeserializer().Deserialize<List<Task>>(response);
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [TestCase("", "Open", "Title cannot be empty!",
            TestName = "Test04_CreateNewTaskInvalidData_NoTitle")]
        public void Test04_CreateNewTaskInvalidData_AssertError(string taskTitle, string boardName, string expectedErrMsg)
        {
            // Arrange
            var request = new RestRequest("/tasks", Method.POST);
            var task = new CreateTask()
            {
                title = taskTitle,
                board = boardName
            };
            request.AddJsonBody(task);

            // Act
            var response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual(expectedErrMsg, errMsg.errMsg);
        }

        [TestCase("Open", TestName = "Test05_CreateNewTaskValidData_BoardNameOpen")]
        [TestCase("In Progress", TestName = "Test05_CreateNewTaskValidData_BoardNameInProgress")]
        [TestCase("Done", TestName = "Test05_CreateNewTaskValidData_BoardNameDone")]
        public void Test05_CreateNewTaskValidData_AssertIsListed(string boardName)
        {
            // Add new Task
            // Arrange

            var taskTitle = "NewTask" + DateTime.Now.Ticks;
            var request = new RestRequest("/tasks", Method.POST);
            var task = new CreateTask
            {
                title = taskTitle,
                description = "ApiTest " + boardName,
                board = boardName
            };
            request.AddJsonBody(task);

            // Act
            var response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var createdTaskResponse = new JsonDeserializer().Deserialize<CreateTaskResponse>(response);
            var createdTask = createdTaskResponse.task;


            //Check for Correct Listing
            // Arrange
            var expectedStr = new JsonDeserializer().Serialize(createdTask);
            request = new RestRequest("/tasks/search/{keyword}", Method.GET);
            request.AddUrlSegment("keyword", taskTitle);

            // Act
            response = this._client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var tasks = new JsonDeserializer().Deserialize<List<Task>>(response);
            Assert.That(tasks.Count, Is.GreaterThan(0));

            var searchedTask = tasks[tasks.Count - 1];
            var actualStr = new JsonDeserializer().Serialize(searchedTask);
            Assert.AreEqual(expectedStr, actualStr);
            Assert.AreEqual(createdTask.board.name, searchedTask.board.name);

        }
    }
}