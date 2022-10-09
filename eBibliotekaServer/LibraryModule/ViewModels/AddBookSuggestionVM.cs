using eBibliotekaServer.AuthModule.Models;
using eBibliotekaServer.LibraryModule.Models;
using System.ComponentModel.DataAnnotations;

namespace eBibliotekaServer.LibraryModule.ViewModels
{
    public class AddBookSuggestionVM
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
