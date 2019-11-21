using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorDbRepository : IBookstoreRepository<Author>
    {
        BookStoreDbContext db;
        
        public AuthorDbRepository(BookStoreDbContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
            // entity.Id = db.Authors.Max(b => b.Id) + 1;
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
        }

        public Author Find(int id)
        {
            var author = db.Authors.SingleOrDefault(b => b.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            //var author = db.Authors.SingleOrDefault(b => b.Id == id);
            //author.FullName = newAuthor.FullName;
            db.Update(newAuthor);
            db.SaveChanges();
        }
        public List<Author> Search(string term)
        {
            if(term != null)
            {
                return db.Authors.Where(a => a.FullName.Contains(term)).ToList();
            }
            return db.Authors.ToList();
        }
    }
}