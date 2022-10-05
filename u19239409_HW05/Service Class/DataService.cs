using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using u19239409_HW05.Models;

namespace u19239409_HW05.Service_Class
{
    public class DataService
    {
        private string ConnectionString;
        SqlConnection DBConnection;
        public DataService()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public bool openConnection()
        {
            bool status = true;
            try
            {
                DBConnection = new SqlConnection(ConnectionString);
                DBConnection.Open();
            }
            catch (Exception e)
            {
                status = false;
                Console.WriteLine(e.StackTrace);
            }
            return status;
        }

        public List<Book> getAllBooks()
        {
            List<Book> books = new List<Book>();
            //TODO: get all books and display them
            openConnection();
            SqlCommand command = new SqlCommand("select books.bookId, books.name as BookName, authors.surname as AuthorName, types.name as TypeName, books.pagecount, books.point " +
            "from books inner join authors on books.authorId = authors.authorId " +
            "inner join types on books.typeId = types.typeId " +
            "group by books.bookId, books.name, authors.surname, types.name, books.pagecount, books.point having count(*) = 1;", DBConnection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Book book = new Book();
                    book.bookId = Convert.ToInt32(reader["bookId"]);
                    book.bookname = reader["BookName"].ToString();
                    book.authorname = reader["AuthorName"].ToString();
                    book.typename = reader["TypeName"].ToString();
                    book.pagecount = Convert.ToInt32(reader["pagecount"]);
                    book.point = Convert.ToInt32(reader["point"]);
                    book.status = "Available";
                 
                    books.Add(book);
                }
            }
            closeConnection();
            return books;
        }

        public List<Student> getAllStudents()
        {
            List<Student> students = new List<Student>();
            //TODO: get all books and display them
            openConnection();
            SqlCommand command = new SqlCommand("select * from students", DBConnection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Student student = new Student();
                    student.studentId = Convert.ToInt32(reader["studentId"]);
                    student.studentname = reader["name"].ToString();
                    student.studentsurname = reader["surname"].ToString();
                    student.Class = reader["class"].ToString();
                    student.point = Convert.ToInt32(reader["point"]);

                    students.Add(student);
                }
            }
            closeConnection();
            return students;
        }


        public List<Book> searchBookByNameTypeAndAuthor(string bookName, string selcType, string selcAuthor)
        {
            List<Book> ListofBooks = getAllBooks();
            List<Book> books = new List<Book>();
            foreach(var book in ListofBooks)
            {
                if(book.bookname==bookName || book.typename==selcType || book.authorname==selcAuthor)
                {
                    books.Add(book);
                }
            }
            return books;
        }

        public List<Student> searchStudentByNameAndClass(string stuName, string selcClass)
        {
            List<Student> ListofStudents = getAllStudents();
            List<Student> students = new List<Student>();
            foreach (var student in ListofStudents)
            {
                if (student.studentname == stuName || student.Class == selcClass)
                {
                    students.Add(student);
                }
            }
            return students;
        }

        public List<Borrows> getBorrowLogPerBook(int BookId)
        {
            List<Borrows> SelectedBookBorrowLog = new List<Borrows>();

            //TODO: get borrowlogs of this book and display them
            openConnection();
            string query = @"SELECT borrows.borrowId, borrows.takenDate, borrows.broughtDate, books.bookId, books.name as BookName, CONCAT(students.name,' ',students.surname) as Borrower
                            FROM borrows
                            INNER JOIN books
                            ON books.bookId = borrows.bookId 
                            INNER JOIN students
                            ON students.studentId = borrows.studentId 
                            WHERE borrows.bookId=" + BookId;

            SqlCommand command = new SqlCommand(query,DBConnection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Borrows borrowinfo = new Borrows();
                    borrowinfo.bookid = Convert.ToInt32(reader["bookId"]);
                    borrowinfo.bookname = reader["BookName"].ToString();
                    borrowinfo.borrowId = Convert.ToInt32(reader["borrowId"]);
                    borrowinfo.takendate = Convert.ToDateTime(reader["takenDate"]);
                    var brdate = reader.GetOrdinal("broughtDate");
                    if(!reader.IsDBNull(brdate)) { 
                        borrowinfo.broughtdate = Convert.ToDateTime(reader[brdate]);
                    }   
                    borrowinfo.borrower = reader["Borrower"].ToString();
                    SelectedBookBorrowLog.Add(borrowinfo);
                }
            }
            closeConnection();
            return SelectedBookBorrowLog;
        }

        public void BorrowBook(int BookID, int StudentID)
        {
            openConnection();
            SqlCommand command = new SqlCommand("INSERT INTO [Library].[dbo].[borrows] (studentId,bookId,takenDate) VALUES ('"+ StudentID +"','"+ BookID +"',GETDATE())", DBConnection);

            using (command)
            {
                command.ExecuteNonQuery();
            }
            closeConnection();
        }

        public void ReturnBook(int BorrowID)
        {
            openConnection();
            SqlCommand command = new SqlCommand("UPDATE [Library].[dbo].[borrows] SET broughtDate = GETDATE() where borrowId = "+ BorrowID, DBConnection);

            using (command)
            {
                command.ExecuteNonQuery();
            }
            closeConnection();
        }

        public bool closeConnection()
        {
            if (DBConnection != null)
            {
                DBConnection.Close();
            }
            return true;
        }
    }
}