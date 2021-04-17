using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace ContactBook.ApiTests
{
    public class Tests
    {
        private const string BaseUrl = "https://contactbook.plvasilev.repl.co/api";


        [Test]
        public void Test01_AssertFirstContactIsSteveJobs()
        {
            var client = new RestClient($"{BaseUrl}/contacts");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            Assert.AreEqual(contacts[0].FirstName, "Steve");
            Assert.AreEqual(contacts[0].LastName, "Jobs");

        }

        [Test]
        public void Test02_SearchContact_ValidData()
        {
            var keyWord = "albert";
            var client = new RestClient($"{BaseUrl}/contacts/search/{keyWord}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            Assert.AreEqual(contacts[0].FirstName, "Albert");
            Assert.AreEqual(contacts[0].LastName, "Einstein");

        }

        [Test]
        public void Test03_SearchContact_InValidData()
        {
            var keyWord = Guid.NewGuid().ToString();
            var client = new RestClient($"{BaseUrl}/contacts/search/{keyWord}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));
            Assert.That(contacts.Count == 0);
        }




        [TestCase("", "LastName","email@email.email", "First name cannot be empty!",
            TestName = "Test04_CreateNewContact_NoFirstName")]
        [TestCase("FirstName", "", "email@email.email", "Last name cannot be empty!",
            TestName = "Test04_CreateNewContact_NoLastName")]
        [TestCase("FirstName", "LastName", "", "Invalid email!",
            TestName = "Test04_CreateNewContact_NoEmail")]
        public void Test04_CreateContact_InValidData(string firstName, string lastName, string email,
            string expectedErrMsg)
        {
            var client = new RestClient($"{BaseUrl}/contacts");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                firstName = firstName ,
                lastName = lastName ,
                email = email,
                phone = "",
                comments = ""
            });
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var errMsg = new JsonDeserializer().Deserialize<ErrorMessage>(response);
            Assert.AreEqual(expectedErrMsg, errMsg.ErrMsg);

        }

        [Test]
        public void Test05_CreateContact_ValidData()
        {
            var contact = new ContactRequest
            {
                firstName = "Marie3",
                lastName = "Curie3",
                email = "marie67@gmail.com",
                phone = "+1 800 200 300",
                comments = "Old friend"
            };

            var client = new RestClient($"{BaseUrl}/contacts");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(contact);
        
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newContact = new JsonDeserializer().Deserialize<CreateContactResponse>(response);

            Assert.AreEqual(contact.firstName, newContact.Contact.FirstName);
            Assert.AreEqual(contact.lastName, newContact.Contact.LastName);
            Assert.AreEqual(contact.email, newContact.Contact.Email);
            Assert.AreEqual(contact.phone, newContact.Contact.Phone);
            Assert.AreEqual(contact.comments, newContact.Contact.Comments);


            request = new RestRequest(Method.GET);
            response = client.Execute(request);
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            Assert.AreEqual(contacts[^1].FirstName, contact.firstName);
            Assert.AreEqual(contacts[^1].LastName, contact.lastName);
            Assert.AreEqual(contacts[^1].Email, contact.email);
            Assert.AreEqual(contacts[^1].Phone, contact.phone);
            Assert.AreEqual(contacts[^1].Comments, contact.comments);
        }
    }
}