using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string FullName { get; set; }
    }
}
