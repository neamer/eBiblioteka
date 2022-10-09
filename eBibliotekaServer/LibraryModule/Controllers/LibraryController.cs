using AutoMapper;
using System.Linq;
using eBibliotekaServer.Helpers;
using eBibliotekaServer.LibraryModule.Repositories;
using eBibliotekaServer.LibraryModule.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using eBibliotekaServer.LibraryModule.Models;
using eBibliotekaServer.AuthModule.Repositories;
using eBibliotekaServer.ImageModule.Repositories;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using eBibliotekaServer.Helpers.Validator;
using eBibliotekaServer.MembershipModule.Repositories;
using eBibliotekaServer.LocationModule.Repositories;
using eBibliotekaServer.LocationModule.ViewModels;
using System.Threading.Tasks;
using System.Threading;

namespace eBibliotekaServer.LibraryModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryRepository libraryRepository, ILocationRepository locationRepository, IMembershipRepository membershipRepository, IAuthRepository authRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _locationRepository = locationRepository;
            _authRepository = authRepository;
            _imageRepository = imageRepository;
            _membershipRepository = membershipRepository;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public ActionResult<PagedList<LibraryListVM>> SearchLibraries(string filter, int items_per_page, int page_number = 1)
        {
            var items = _libraryRepository.SearchLibraries(filter);
            List<LibraryListVM> results = new List<LibraryListVM>();

            int userId = AuthHelper.GetAccountIdFromRequest(Request);

            if (userId != -1)
            {
                foreach (var item in items)
                {
                    var mapped = _mapper.Map<LibraryListVM>(item);
                    mapped.IsMember = _membershipRepository.IsMember(item.ID, userId);
                    mapped.NoOfMembers = _libraryRepository.GetMemberCount(item.ID);
                    mapped.NoOfBooks = _libraryRepository.GetBookCount(item.ID);
                    results.Add(mapped);
                }
            } 
            else
            {
                foreach (var item in items)
                {
                    var vm = _mapper.Map<LibraryListVM>(item);
                    vm.NoOfMembers = _libraryRepository.GetMemberCount(item.ID);
                    vm.NoOfBooks = _libraryRepository.GetBookCount(item.ID);
                    results.Add(vm);
                }
            }

            return PagedList<LibraryListVM>.Create(results.AsQueryable(), page_number, items_per_page);
        }

        [HttpGet("search-map")]
        public ActionResult<List<LibraryMapSearch>> SearchLibrariesMap()
        {
            var items = _libraryRepository.SearchLibrariesMap();

            var results = new List<LibraryMapSearch>();

            foreach (var item in items)
            {
                results.Add(_mapper.Map<LibraryMapSearch>(item));
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public ActionResult<LibraryDetailsVM> GetLibraryDetails(int id)
        {
            var item = _libraryRepository.GetLibrary(id);

            if (item == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LibraryDetailsVM>(item);

            var offers = _libraryRepository.GetMembershipOffers(id);

            result.MembershipOffers = new List<MembershipOfferGetVM>();

            foreach (var offer in offers)
            {
                result.MembershipOffers.Add(_mapper.Map<MembershipOfferGetVM>(offer));
            }

            var hours = _libraryRepository.GetBusinessHours(id);

            result.BusinessHours = new List<BusinessHoursGetVM>();

            foreach (var hour in hours)
            {
                result.BusinessHours.Add(_mapper.Map<BusinessHoursGetVM>(hour));
            }

            int userId = AuthHelper.GetAccountIdFromRequest(Request);

            if(userId != -1)
            {
                result.membership = _membershipRepository.GetMember(id, userId);
                if(result.membership != null)
                {
                    result.membership.MembershipOffer.Library = null;
                }
            }

            result.NoOfMembers = _libraryRepository.GetMemberCount(id);
            result.NoOfBooks = _libraryRepository.GetBookCount(id);

            return Ok(result);
        }

        [HttpGet("details/map/{id}")]
        public ActionResult<LibraryListVM> GetLibraryDetailsForMap(int id)
        {
            Thread.Sleep(1000);

            int userId = AuthHelper.GetAccountIdFromRequest(Request);

            var item = _libraryRepository.GetLibrary(id);

            if (item == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LibraryListVM>(item);
            result.Location = null;

            if (userId != -1)
            {
                result.IsMember = _membershipRepository.IsMember(item.ID, userId);
            }


            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public ActionResult<LibraryDetailsVM> GetLibraryAuth()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.GetLibrary(libraryID);

            if (item == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<LibraryDetailsVM>(item);

            var offers = _libraryRepository.GetMembershipOffers(libraryID);

            result.MembershipOffers = new List<MembershipOfferGetVM>();

            foreach (var offer in offers)
            {
                result.MembershipOffers.Add(_mapper.Map<MembershipOfferGetVM>(offer));
            }

            var hours = _libraryRepository.GetBusinessHours(libraryID);

            result.BusinessHours = new List<BusinessHoursGetVM>();

            foreach (var hour in hours)
            {
                result.BusinessHours.Add(_mapper.Map<BusinessHoursGetVM>(hour));
            }

            result.NoOfMembers = _libraryRepository.GetMemberCount(libraryID);
            result.NoOfBooks = _libraryRepository.GetBookCount(libraryID);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public ActionResult<Library> UpdateLibrary([FromBody] LibraryUpdateVM data)
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.UpdateLibrary(libraryID, data);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("profileImage"), DisableRequestSizeLimit]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult> UploadProfileImgImage()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);
            var librarian = _authRepository.GetLibrarian(librarianId);

            if (librarian.Library.ProfileImageID.HasValue)
            {
                try
                {
                    await _imageRepository.RemoveImage(librarian.Library.ProfileImageID.GetValueOrDefault());
                }
                catch (Exception ex)
                {
                    throw new Exception("Greska prilikom brisanja slike");
                }
            }

            try
            {
                var path = await _imageRepository.UploadImage(Request.Form.Files[0]);

                _imageRepository.AddLibraryProfileImage(librarian.Library, path);

                return Ok();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest("Invalid file");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("banner"), DisableRequestSizeLimit]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult> UploadNewBannerImage()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);
            var librarian = _authRepository.GetLibrarian(librarianId);

            if (librarian.Library.BannerImageID.HasValue)
            {
                try
                {
                    await _imageRepository.RemoveImage(librarian.Library.BannerImageID.GetValueOrDefault());

                }
                catch (Exception ex)
                {
                    throw new Exception("Greska prilikom brisanja slike");
                }
            }

            try
            {
                var path = await _imageRepository.UploadImage(Request.Form.Files[0]);

                _imageRepository.AddLibraryBannerImage(librarian.Library, path);
                _imageRepository.SaveChanges();

                return Ok();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest("Invalid file");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("profileImage")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult> RemoveProfileImage()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);
            var librarian = _authRepository.GetLibrarian(librarianId);

            if (librarian.Library.ProfileImageID.HasValue)
            {
                try
                {
                    await _imageRepository.RemoveImage(librarian.Library.ProfileImageID.GetValueOrDefault());
                    _imageRepository.RemoveLibraryProfileImage(librarian.Library);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [HttpGet("banner")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult> RemoveBanner()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);
            var librarian = _authRepository.GetLibrarian(librarianId);

            if (librarian.Library.BannerImageID.HasValue)
            {
                try
                {
                    await _imageRepository.RemoveImage(librarian.Library.BannerImageID.GetValueOrDefault());
                    _imageRepository.RemoveLibraryBannerImage(librarian.Library);

                    return Ok();

                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [HttpPost("offers")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<Library> CreateOffer([FromBody] MembershipOfferCreateVM data)
        {
            var errors = Validator.ValidateMembershipOffer(_mapper.Map<MembershipOffer>(data));

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.CreateMembershipOffer(libraryID, data);

            return Ok(item);
        }

        [HttpGet("offers")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<MembershipOfferGetVM> GetOffersForLibrary()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.GetMembershipOffers(libraryID);

            return Ok(item);
        }

        [HttpGet("offers/{id}/delete")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<Library> DeleteOffer(int id)
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.GetMembershipOffer(id);

            if (item.LibraryID != libraryID) return Unauthorized();

            item = _libraryRepository.DeleteMembershipOffer(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("offers/{id}")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<Library> UpdateOffer(int id, [FromBody] MembershipOfferUpdateVM data)
        {
            var errors = Validator.ValidateMembershipOffer(_mapper.Map<MembershipOffer>(data));

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.GetMembershipOffer(id);

            if (item.LibraryID != libraryID) return Unauthorized();

            item = _libraryRepository.UpdateMembershipOffer(id, data);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("business-hours/{id}")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<BusinessHours> PostBusinessHours(int id, [FromBody]BusinessHoursCreateVM data)
        {
            var errors = Validator.ValidateBusinessHours(data);

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            if (id == -1)
            {
                var item = _libraryRepository.CreateBusinessHours(libraryID, data);

                return Ok(item);
            } else
            {
                var item = _libraryRepository.GetBusinessHour(id);

                if (item.LibraryID != libraryID) return Unauthorized();

                item = _libraryRepository.UpdateBusinessHours(id, data);

                if(item == null)
                {
                    return BadRequest();
                }

                if (item.LibraryID != libraryID) return Unauthorized();

                return Ok(item);
            }

        }

        [HttpGet("business-hours/{id}/delete")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<BusinessHours> DeleteBusinessHours(int id)
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var item = _libraryRepository.GetBusinessHour(id);

            if (item.LibraryID != libraryID) return Unauthorized();

            item = _libraryRepository.DeleteBusinessHours(id);

            if (item.LibraryID != libraryID) return Unauthorized();

            return Ok(item);
        }

        [HttpGet("for-user")]
        [Authorize(Roles = "User")]
        public ActionResult<LibraryDetailsVM> GetLibrariesForUser()
        {
            int userId = AuthHelper.GetAccountIdFromRequest(Request);

            var items = _libraryRepository.GetLibrariesForUser(userId);

            var results = new List<UserLibrariesVM>();

            foreach(var item in items)
            {
                var vm = _mapper.Map<UserLibrariesVM>(item);
                vm.NoOfMembers = _libraryRepository.GetMemberCount(item.ID);
                vm.NoOfBooks = _libraryRepository.GetBookCount(item.ID);
                results.Add(vm);
            }

            return Ok(results);
        }
        [HttpPost("send-notification")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<Notification> SendNotification([FromBody] SendNotificationVM data) 
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var result = _libraryRepository.SendNotification(data, libraryID);
            return Ok(result);
        }

        [HttpGet("get-notifications-for-user")]
        [Authorize(Roles = "User")]
        public ActionResult<List<NotificationListItemVM>> GetNotificationsForUser()
        {
            int userId = AuthHelper.GetAccountIdFromRequest(Request);
            List<NotificationListItemVM> results = new List<NotificationListItemVM>();
            var items = _libraryRepository.GetNotificationsForUser(userId);
            foreach (var item in items)
            {
                results.Add(_mapper.Map<NotificationListItemVM>(item));
            }
            if(results.Count == 0)
            {
                return Ok();
            }
            return Ok(results);
        }


        [HttpGet("get-notifications-homepage")]
        [Authorize(Roles = "User")]
        public ActionResult<List<NotificationListItemVM>> GetNotificationsHomepage()
        {
            int userId = AuthHelper.GetAccountIdFromRequest(Request);
            List<NotificationListItemVM> results = new List<NotificationListItemVM>();
            var items = _libraryRepository.GetNotificationsForUser(userId).Take(5);
            foreach (var item in items)
            {
                results.Add(_mapper.Map<NotificationListItemVM>(item));
            }
            return Ok(results);
        }

        [HttpGet("get-notification")]
        [Authorize(Roles = "User")]
        public ActionResult<Notification> GetNotification(int id)
        {
           var item = _libraryRepository.GetNotification(id);
            if (item == null)
            {
                return NotFound();

            }
            else if(item.RecipientID != id)
            {
                return Unauthorized();
            }
            return Ok(item);
        }

        [HttpGet("remove-notification/{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<bool> RemoveNotification(int id)
        {
            int userId = AuthHelper.GetAccountIdFromRequest(Request);
            var item = _libraryRepository.GetNotification(id);
            if (item.RecipientID != userId)
            {
                return Unauthorized();
            }
            else
            {
                _libraryRepository.RemoveNotification(id);
                return Ok(true);
            }
        }


        [HttpPost("book-suggestion/{libraryID}")]
        [Authorize(Roles = "User")]
        public ActionResult<BookSuggestionGetVM> AddBookSuggestion(int libraryID, [FromBody]AddBookSuggestionVM data)
        {
            int userId = AuthHelper.GetAccountIdFromRequest(Request);
            var item = _libraryRepository.AddBookSuggestion(libraryID, userId, data);

            var errors = Validator.ValidateBookSuggestion(data);

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            return Ok(_mapper.Map< BookSuggestionGetVM > (item));
        }

        [HttpGet("book-suggestion/{suggestionId}/delete")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<BookSuggestionGetVM> DeleteBookSuggestion(int suggestionId)
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var suggestion = _libraryRepository.GetBookSuggestion(suggestionId);

            if(suggestion == null)
            {
                return NotFound();
            }

            if(suggestion.LibraryID != libraryID)
            {
                return Unauthorized();
            }

            var item = _libraryRepository.DeleteBookSuggestion(suggestionId);

            return Ok(_mapper.Map<BookSuggestionGetVM>(item));
        }

        [HttpGet("book-suggestion/")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<BookSuggestionGetVM> GetBookSuggestions()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var suggestions = _libraryRepository.GetBookSuggestions(libraryID);

            return Ok(_mapper.Map<List<BookSuggestionGetVM>>(suggestions));
        }

        [HttpGet("book-suggestion-homepage/")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<BookSuggestionGetVM> GetBookSuggestionsHomepage()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var suggestions = _libraryRepository.GetBookSuggestions(libraryID).Take(3);

            return Ok(_mapper.Map<List<BookSuggestionGetVM>>(suggestions));
        }

        [HttpGet("stats-homepage/")]
        [Authorize(Roles = "Librarian")]
        public ActionResult<LibraryStats> GetStatsHomepage()
        {
            int librarianId = AuthHelper.GetAccountIdFromRequest(Request);

            int libraryID = _authRepository.GetLibrarian(librarianId).LibraryID;

            var result = new LibraryStats()
            {
                NoOfMembers = _libraryRepository.GetMemberCount(libraryID),
                NoOfBooks = _libraryRepository.GetBookCount(libraryID),
            };

            return Ok(result);
        }
    }
}
