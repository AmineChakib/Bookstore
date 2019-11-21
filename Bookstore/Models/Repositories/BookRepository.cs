using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    
    public class BookRepository : IBookstoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id=1,Title="C# Programming",Description="No description",Author=new Author(),ImageUrl="C_Sharp.jpg"
                },
                new Book
                {
                    Id=2,Title="Java Programming",Description="No description",Author=new Author(),ImageUrl="Java.jpg"
                },
                new Book
                {
                    Id=3,Title="Python Programming",Description="No description",Author=new Author(),ImageUrl="Python.jpg"
                },
                new Book
                {
                    Id=4,Title="C Programming",Description="No description",Author=new Author(),ImageUrl="C.jpg"
                },
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update(int id,Book newBook)
        {
            var book = Find(id);
            book.Author = newBook.Author;
            book.Description = newBook.Description;
            book.Title = newBook.Title;
            book.ImageUrl = newBook.ImageUrl;
        }
        public List<Book> Search(string term)
        {
            var result = books.Where(b => b.Title.Contains(term)
               || b.Description.Contains(term)
               || b.Author.FullName.Contains(term)).ToList();
            return result;
        }
    }
}
