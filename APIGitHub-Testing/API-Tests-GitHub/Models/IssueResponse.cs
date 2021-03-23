using System;
using System.Collections.Generic;
using System.Text;

namespace API_Tests_GitHub.Models
{
    public class IssueResponse
    {
        public long Id { get; set; }
        public long Number { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
    }
}
