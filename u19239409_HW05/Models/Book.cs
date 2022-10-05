using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19239409_HW05.Models
{
    public class Book
    {
        public int bookId { get; set; }

        public string bookname { get; set; }

        public int pagecount { get; set; }

        public int point { get; set; }

        public string authorname { get; set; }

        public string typename { get; set; }

        public string status { get; set; }
    }
}