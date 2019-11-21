using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        public IBookstoreRepository<Book> bookRepository;

        public IBookstoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookstoreRepository<Book> bookRepository,
            IBookstoreRepository<Author> authorRepository,
            IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View(GetAllAuthors());
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {               
                try
                {
                    string fileName = UploadFile(model.File) ?? string.Empty;
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author from the list !!";
                        return View(GetAllAuthors());
                        
                    }
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Author = authorRepository.Find(model.AuthorId),
                        Description = model.Description,
                        Title = model.Title,
                        ImageUrl = fileName
                    };
                    // TODO: Add insert logic here
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "You have to fill the required fields!!");
            return View(GetAllAuthors());
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors=authorRepository.List().ToList(),
                ImageUrl=book.ImageUrl
            };
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = UploadFile(viewModel.File,viewModel.ImageUrl);
                //if (viewModel.File != null)
                //{
                //    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                //    fileName = viewModel.File.FileName;
                //    string fullPath = Path.Combine(uploads, fileName);
                //    //Delete the old file
                //    string oldFileName = viewModel.ImageUrl; //bookRepository.Find(viewModel.BookId).ImageUrl;
                //    string oldFullPath = Path.Combine(uploads, oldFileName);
                //    if (fullPath != oldFullPath)
                //    {
                //        System.IO.File.Delete(oldFullPath);
                //        //Save the new File
                //        viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                //    }
                //}
                // TODO: Add update logic here
                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book
                {
                    Id = viewModel.BookId,
                    Author = author,
                    Description = viewModel.Description,
                    Title = viewModel.Title,
                    ImageUrl= fileName
                };
                bookRepository.Update(viewModel.BookId, book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please select an author--" });
            return authors;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;
        }
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
                return file.FileName;
            }
            else
            {
                return null;
            }
        }
        string UploadFile(IFormFile file,string ImageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                //Delete the old file
                //bookRepository.Find(viewModel.BookId).ImageUrl;
                string oldFullPath = Path.Combine(uploads, ImageUrl);
                if (fullPath != oldFullPath)
                {
                    System.IO.File.Delete(oldFullPath);
                    //Save the new File
                    file.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                return file.FileName;
            }
            return ImageUrl; 
        }
        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);
            return View("Index", result);
        }
    }
}