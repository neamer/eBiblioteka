using eBibliotekaServer.AuthModule.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eBibliotekaServer.LibraryModule.Models
{
    public class BookSuggestion
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int LibraryID { get; set; }
        public virtual Library Library { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}
