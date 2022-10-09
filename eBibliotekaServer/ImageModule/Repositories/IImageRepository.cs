using eBibliotekaServer.AuthModule.Models;
using eBibliotekaServer.BookModule.Models;
using eBibliotekaServer.ImageModule.Models;
using eBibliotekaServer.LibraryModule.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace eBibliotekaServer.ImageModule.Repositories
{
    public interface IImageRepository
    {
        public Image AddImage(string url);

        public void AddLibraryProfileImage(Library library, string url);
        public void RemoveLibraryProfileImage(Library library);

        public void AddLibraryBannerImage(Library library, string url);
        public void RemoveLibraryBannerImage(Library library);

        public void AddBookCoverImage(Book library, string url);
        public void RemoveBookCoverImage(Book library);

        public void AddLibrarianProfileImage(Librarian library, string url);
        public void RemoveLibrarianProfileImage(Librarian library);

        public void AddUserProfileImage(User library, string url);
        public void RemoveUserProfileImage(User library);


        public Task<string> UploadImage(IFormFile image);
        public Task RemoveImage(int id);

        public bool SaveChanges();
    }
}
