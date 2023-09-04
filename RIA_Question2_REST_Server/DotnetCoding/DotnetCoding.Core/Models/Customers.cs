

using System.ComponentModel.DataAnnotations;

namespace DotnetCoding.Core.Models
{
    public class Customers
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
