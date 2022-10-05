using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19239409_HW05.Models
{
    public class BorrowsVM
    {
        public List<Borrows> BorrowLogs { get; set; }

        public string SelectedBookName;

        public string SelectedBookStatus;

        public int SelectedBookID;
    }
}