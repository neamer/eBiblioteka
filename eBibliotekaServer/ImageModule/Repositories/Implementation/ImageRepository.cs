using eBibliotekaServer.AuthModule.Models;
using eBibliotekaServer.BookModule.Models;
using eBibliotekaServer.Data;
using eBibliotekaServer.ImageModule.Models;
using eBibliotekaServer.LibraryModule.Models;
using Google.Apis.Upload;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace eBibliotekaServer.ImageModule.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public ImageRepository(AppDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }

        public Image AddImage(string url)
        {
            var image = new Image() { Path = url, CreatedAt = DateTime.Now };

            _context.Images.Add(image);
            _context.SaveChanges();

            return image;
        }        
        
        public void AddLibraryProfileImage(Library library, string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var image = new Image() { Path = url, CreatedAt = DateTime.Now };

                    (scopedContext as AppDbContext).Images.Add(image);
                    (scopedContext as AppDbContext).SaveChanges();

                    var libraryDb = (scopedContext as AppDbContext).Libraries.Find(library.ID);

                    libraryDb.ProfileImageID = image.ID;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }

        public void RemoveLibraryProfileImage(Library library)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var libraryDb = (scopedContext as AppDbContext).Libraries.Find(library.ID);

                    libraryDb.ProfileImageID = null;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }

        public void AddLibraryBannerImage(Library library, string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var image = new Image() { Path = url, CreatedAt = DateTime.Now };

                    (scopedContext as AppDbContext).Images.Add(image);
                    (scopedContext as AppDbContext).SaveChanges();

                    var libraryDb = (scopedContext as AppDbContext).Libraries.Find(library.ID);

                    libraryDb.BannerImageID = image.ID;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }
        public void RemoveLibraryBannerImage(Library library)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var libraryDb = (scopedContext as AppDbContext).Libraries.Find(library.ID);

                    libraryDb.BannerImageID = null;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }

        public void AddBookCoverImage(Book book, string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var image = new Image() { Path = url, CreatedAt = DateTime.Now };

                    (scopedContext as AppDbContext).Images.Add(image);
                    (scopedContext as AppDbContext).SaveChanges();

                    var bookDb = (scopedContext as AppDbContext).Books.Find(book.ID);

                    bookDb.CoverImageID = image.ID;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }
        public void RemoveBookCoverImage(Book book)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var bookDb = (scopedContext as AppDbContext).Books.Find(book.ID);

                    bookDb.CoverImageID = null;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }

        public void AddLibrarianProfileImage(Librarian librarian, string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var image = new Image() { Path = url, CreatedAt = DateTime.Now };

                    (scopedContext as AppDbContext).Images.Add(image);
                    (scopedContext as AppDbContext).SaveChanges();

                    var librarianDb = (scopedContext as AppDbContext).Librarians.Find(librarian.ID);

                    librarianDb.ProfileImageID = image.ID;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }
        public void RemoveLibrarianProfileImage(Librarian librarian)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var librarianDb = (scopedContext as AppDbContext).Librarians.Find(librarian.ID);

                    librarianDb.ProfileImageID = null;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }

        public void AddUserProfileImage(User user, string url)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var image = new Image() { Path = url, CreatedAt = DateTime.Now };

                    (scopedContext as AppDbContext).Images.Add(image);
                    (scopedContext as AppDbContext).SaveChanges();

                    var userDb = (scopedContext as AppDbContext).Users.Find(user.ID);

                    userDb.ProfileImageID = image.ID;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }
        public void RemoveUserProfileImage(User user)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetService(typeof(AppDbContext));

                    var userDb = (scopedContext as AppDbContext).Librarians.Find(user.ID);

                    userDb.ProfileImageID = null;
                    (scopedContext as AppDbContext).SaveChanges();
                }
            }));
        }


        public async Task<string> UploadImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                // STARI KOD - Upravljanje slikama prebačeno na google cloud
                //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //var fullPath = Path.Combine(pathToSave, fileName);
                //var dbPath = Path.Combine(folderName, fileName);

                //using (var stream = new FileStream(fullPath, FileMode.Create))
                //{
                //    file.CopyTo(stream);
                //}

                var newObject = new Google.Apis.Storage.v1.Data.Object
                {
                    Bucket = GoogleConfig.BucketName,
                    Name = Helpers.Generator.RandomString(50),
                    ContentType = "image/jpeg"
                };

                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(GoogleConfig.credentialJSON);


                // Instantiates a client.
                using (var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(credential))
                {
                    try
                    {
                        var ms = new MemoryStream();

                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);


                        // Creates the new bucket. Only required the first time.
                        // You can also create buckets through the GCP cloud console web interface
                        //storageClient.CreateBucket(GoogleConfig.ProjectId, GoogleConfig.BucketName);

                        // set minimum chunksize just to see progress updating
                        var uploadObjectOptions = new Google.Cloud.Storage.V1.UploadObjectOptions
                        {
                            ChunkSize = Google.Cloud.Storage.V1.UploadObjectOptions.MinimumChunkSize,
                        };

                        var storageObject = await storageClient.UploadObjectAsync(
                                newObject,
                                ms,
                                uploadObjectOptions)
                        .ConfigureAwait(false);

                        return storageObject.Name;
                    }


                    catch (Exception e)
                    {
                        throw new InvalidDataException();
                    }
                }
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        public async Task RemoveImage(int id)
        {
            try
            {
                var image = _context.Images.FirstOrDefault(i => i.ID == id);


                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(GoogleConfig.credentialJSON);

                using (var storageClient = Google.Cloud.Storage.V1.StorageClient.Create(credential))
                {
                    await storageClient.DeleteObjectAsync(GoogleConfig.BucketName, image.Path);

                    _context.Remove(image);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new Exception("42");
            }
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
