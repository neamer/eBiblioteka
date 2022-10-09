using eBibliotekaServer.AuthModule.Models;
using eBibliotekaServer.AuthModule.ViewModels;
using eBibliotekaServer.LibraryModule.Models;
using System.ComponentModel.DataAnnotations;

namespace eBibliotekaServer.LibraryModule.ViewModels
{
    public class BookSuggestionGetVM
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int UserID { get; set; }
        public virtual AccountGetVM User { get; set; }
    }
}
