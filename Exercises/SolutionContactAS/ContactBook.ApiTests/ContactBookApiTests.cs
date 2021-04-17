using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ContactBook.ApiTests
{
    public class ContactBookApiTests
    {
        const string ApiBaseUrl = "https://contactbook.nakov.repl.co/api";
        RestClient client;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(ApiBaseUrl);
        }

        [Test]
        public void Test_ListContacts_CheckForSteveJobs()
        {
            // Arrange
            var request = new RestRequest("/contacts", Method.GET);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.That(contacts.Count, Is.GreaterThan(0));
            var firstContact = contacts[0];
            Assert.That(firstContact.firstName, Is.EqualTo("Steve"));
            Assert.That(firstContact.lastName, Is.EqualTo("Jobs"));
        }

        [Test]
        public void Test_FindContact_AlbertEinstein()
        {
            // Arrange
            var request = new RestRequest("/contacts/search/{keyword}", Method.GET);
            request.AddUrlSegment("keyword", "albert");

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.That(contacts.Count, Is.GreaterThan(0));
            var firstContact = contacts[0];
            Assert.That(firstContact.firstName, Is.EqualTo("Albert"));
            Assert.That(firstContact.lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_FindContact_Invalid()
        {
            // Arrange
            var request = new RestRequest("/contacts/search/{keyword}", Method.GET);
            request.AddUrlSegment("keyword", "invalid2635");

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = new JsonDeserializer().Deserialize<List<Contact>>(response);
            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateContact_InvalidData()
        {
            // Arrange
            var request = new RestRequest("/contacts", Method.POST);
            request.AddJsonBody(new { 
                firstName = "invalid data -> no lastName",
                phone = "04387742332",
                email = "email@abv.bg"
            });

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Test_CreateContact_ValidData()
        {
            // Arrange
            var request = new RestRequest("/contacts", Method.POST);
            var newContact = new
            {
                firstName = "fisrtName" + DateTime.Now.Ticks,
                lastName = "lastName" + DateTime.Now.Ticks,
                phone = "04387742332",
                email = "email@abv.bg"
            };
            request.AddJsonBody(newContact);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var requestAllContacts = new RestRequest("/contacts", Method.GET);
            var responseAllContacts = this.client.Execute(requestAllContacts);

            // Assert
            Assert.That(responseAllContacts.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var allContacts = new JsonDeserializer().Deserialize<List<Contact>>(responseAllContacts);
            Assert.That(allContacts.Count, Is.GreaterThan(0));
            var lastContact = allContacts[allContacts.Count - 1];
            Assert.That(lastContact.firstName, Is.EqualTo(newContact.firstName));
            Assert.That(lastContact.lastName, Is.EqualTo(newContact.lastName));
        }
    }
}