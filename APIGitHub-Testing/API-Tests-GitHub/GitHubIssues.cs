using System;
using System.Collections.Generic;
using System.Net;
using API_Tests_GitHub.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace API_Tests_GitHub
{
    public class GitHubIssues
    {
        const string GitHubAPIUsername = "YOUR USERNAME";
        const string GitHubAPIPass = "YOUR TOKEN";
 
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void _01_Retrieve_All_Issues_from_Repo()
        {
            var client = new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues") { Timeout = 3000 };
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var issues = new JsonDeserializer().Deserialize<List<IssueResponse>>(response);
            foreach (var issue in issues)
            {
                string idType = issue.Id.GetType().Name;
                Assert.AreEqual(idType, "Int64");

                string numberType = issue.Number.GetType().Name;
                Assert.AreEqual(numberType, "Int64");

                string titleType = issue.Title.GetType().Name;
                Assert.AreEqual(titleType, "String");

                string bodyType = issue.Body.GetType().Name;
                Assert.AreEqual(bodyType, "String");
            }
        }

        [Test]
        public void _02_Retrieve_Issue_by_Number()
        {
            var issueNumber = 9;
            var client = new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues/" + issueNumber) { Timeout = 3000 };
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var issue = new JsonDeserializer().Deserialize<IssueResponse>(response);

            string idType = issue.Id.GetType().Name;
            Assert.AreEqual(idType, "Int64");
            Assert.IsTrue(issue.Id > 0);

            string numberType = issue.Number.GetType().Name;
            Assert.AreEqual(numberType, "Int64");
            Assert.IsTrue(issue.Number > 0);

            string titleType = issue.Title.GetType().Name;
            Assert.AreEqual(titleType, "String");
            Assert.IsTrue(!string.IsNullOrEmpty(issue.Title));

            string bodyType = issue.Body.GetType().Name;
            Assert.AreEqual(bodyType, "String");
            Assert.IsTrue(!string.IsNullOrEmpty(issue.Body));
        }

        [Test]
        public void _03_Create_a_New_Issue()
        {
            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues") { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = "issue C# Plamen",
                body = "Test issue body",
                labels = new string[] { "bug", "importance:high", "type:UI" }
            });

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var issue = new JsonDeserializer().Deserialize<IssueResponse>(response);

            string idType = issue.Id.GetType().Name;
            Assert.AreEqual(idType, "Int64");
            Assert.IsTrue(issue.Id > 0);

            string numberType = issue.Number.GetType().Name;
            Assert.AreEqual(numberType, "Int64");
            Assert.IsTrue(issue.Number > 0);

            string titleType = issue.Title.GetType().Name;
            Assert.AreEqual(titleType, "String");
            Assert.IsTrue(!string.IsNullOrEmpty(issue.Title));

            string bodyType = issue.Body.GetType().Name;
            Assert.AreEqual(bodyType, "String");
            Assert.IsTrue(!string.IsNullOrEmpty(issue.Body));

        }

        [Test]
        public void _03_Create_a_New_Issue_Unauthorized()
        {
            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues") {Timeout = 3000};
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = "some title",
                body = "some body",
                labels = new string[] { "bug", "importance:high", "type:UI" }
            });
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public void _03_Create_a_New_Issue_Empty_Title_And_Body()
        {
            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues") { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { });
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Test]
        public void _03_Create_a_New_Issue_Empty_Title()
        {
            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues") { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { body = "some body" });
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Test]
        public void _04_Edit_Existing_Issue()
        {
            var issueNumber = 2709;
            string newTitle = "issue C# Plamen Edited 4";
            string newBody = "Test issue body Edited 4";

            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues/" + issueNumber) { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = newTitle,
                body = newBody,
            });

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var issue = new JsonDeserializer().Deserialize<IssueResponse>(response);

            Assert.AreEqual(newTitle, issue.Title);
            Assert.AreEqual(newBody, issue.Body);
        }

        [Test]
        public void _04_Edit_Non_existing_ID_Issue()
        {
            var issueNumber = 27091221;
            string newTitle = "issue C# Plamen Edited 4";
            string newBody = "Test issue body Edited 4";

            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues/" + issueNumber) { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = newTitle,
                body = newBody,
            });

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Test]
        public void _04_Edit_Non_existing_ID_Issue_Unauthorized()
        {
            var issueNumber = 2709;
            string newTitle = "issue C# Plamen Edited 4";
            string newBody = "Test issue body Edited 4";

            var client =
                new RestClient("https://api.github.com/repos/testnakov/test-nakov-repo/issues/" + issueNumber) { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = newTitle,
                body = newBody,
            });

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

        }

        [Test]
        public void _05_CreateAndDeleteComment()
        {
            var id = "2709";
            var client =
                new RestClient($"https://api.github.com/repos/testnakov/test-nakov-repo/issues/{id}/comments") { Timeout = 3000 };
            var request = new RestRequest(Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(GitHubAPIUsername, GitHubAPIPass);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { body = "My new comment" + DateTime.UtcNow });
            var response = client.Execute(request);
            var comment = new JsonDeserializer().Deserialize<Comment>(response);

            client = new RestClient($"https://api.github.com/repos/testnakov/test-nakov-repo/issues/comments/{comment.Id}") { Timeout = 3000 };
            response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
        }
    }
}