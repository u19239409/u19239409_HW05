using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19239409_HW05.Models
{
    public class Borrows
    {
        public int borrowId { get; set; }

        public int bookid { get; set; }

        public DateTime takendate { get; set; }

        public DateTime broughtdate { get; set; }

        public string borrower { get ; set; }

        public string bookname { get; set; }
    }
}