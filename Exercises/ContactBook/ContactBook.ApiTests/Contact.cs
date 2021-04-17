using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBook.ApiTests
{
    public class Contact
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string DateCreated { get; set; }

        public string Comments { get; set; }

    }
}
