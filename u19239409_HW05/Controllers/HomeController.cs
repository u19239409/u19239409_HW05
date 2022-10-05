using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u19239409_HW05.Service_Class;
using u19239409_HW05.Models;

namespace u19239409_HW05.Controllers
{
    public class HomeController : Controller
    {
        private DataService DataService = new DataService();

        public ActionResult Index()
        {
            List<Book> Books = DataService.getAllBooks();
            return View(Books);
        }

        [HttpPost]
        public ActionResult Index(string bookName, string selcType, string selcAuthor)
        {
            List<Book> Books = DataService.searchBookByNameTypeAndAuthor(bookName, selcType, selcAuthor);
            return View(Books);
        }

        public string getBookName(int BookId)
        {
            List<Book> Books = DataService.getAllBooks();

            var bookName = "";
            foreach (var book in Books)
            {
                if (book.bookId == BookId)
                {
                    bookName = book.bookname;
                }
            }
            return bookName;
        }

        public string getBookStatus(int BookId)
        {
            List<Book> Books = DataService.getAllBooks();
            var status="";
            foreach (var book in Books)
            {
                if (book.bookId == BookId)
                {
                    status = book.status;
                }
            }
            return status;
        }

        public ActionResult ViewBookDetails(int BookId)
        {
            BorrowsVM viewModel = new BorrowsVM();
            viewModel.BorrowLogs = DataService.getBorrowLogPerBook(BookId);
            viewModel.SelectedBookName = getBookName(BookId);
            viewModel.SelectedBookStatus = getBookStatus(BookId);
            viewModel.SelectedBookID = BookId;
            TempData["BookID"] = BookId;
            TempData.Keep("BookID");
            return View(viewModel);
        }
    
        public ActionResult ViewStudents()
        {
            List<Student> Students = DataService.getAllStudents();
            ViewData["BookID"] = TempData.Peek("BookID");
            return View(Students);
        }

        [HttpPost]
        public ActionResult ViewStudents(string stuName, string selcClass)
        {
            List<Student> Students = DataService.searchStudentByNameAndClass(stuName,selcClass);
            return View(Students);
        }

        [HttpGet]
        public ActionResult BorrowBook(int BookID,int StudentID)
        {
            DataService.BorrowBook(BookID,StudentID);
            return RedirectToAction("ViewBookDetails", new {BookId = BookID});
        }

        [HttpGet]
        public ActionResult ReturnBook(int BookID)
        {
            List<Borrows> Borrows = DataService.getBorrowLogPerBook(BookID);
            DataService.ReturnBook(Borrows.Where(zz => zz.bookid == BookID).FirstOrDefault().borrowId);
            return RedirectToAction("ViewBookDetails", new { BookId = BookID });
        }
    }
}