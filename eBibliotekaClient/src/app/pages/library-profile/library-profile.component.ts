import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { HttpConfig } from '../../configs/HttpConfig';
import { AuthService } from '../../core/auth/auth.service';
import { BookService } from '../../core/http/book.service';
import { LibraryService } from '../../core/http/library.service';
import { AccountType } from '../../data/enums/AccountType';
import { BookSearchVM } from '../../data/interfaces/Book';
import {
  BusinessHoursEdit,
  BusinessHours,
  convertToBusinessHoursEdit,
} from '../../data/interfaces/BusinessHours';
import { Image } from '../../data/interfaces/Image';
import { Library, LibraryUpdateVM } from '../../data/interfaces/Library';
import { MapLocation } from '../../data/interfaces/MapLocation';
import { MembershipOffer } from '../../data/interfaces/MembershipOffer';
import { ImageUploadOptions } from '../../data/types/ImageUploadOptions';
import { createImgPath } from '../../helpers/CreateImgPath';
import { MembershipOfferType } from './membership-offer/MembershipOfferType';

@Component({
  selector: 'app-library-profile',
  templateUrl: './library-profile.component.html',
  styleUrls: [
    './library-profile.component.css',
    '../library-detail/library-detail.component.css',
  ],
})
export class LibraryProfileComponent implements OnInit {
  library: Library;

  editing: boolean = false;
  libraryEdit: LibraryUpdateVM;
  updateLoading: boolean = false;

  booksRecommendations: BookSearchVM[];
  recommendationsID: number = -1;
  bannerUploadOptions: ImageUploadOptions;
  profileImgUploadOptions: ImageUploadOptions;
  showBannerUpload: boolean = false;
  showProfileImgUpload: boolean = false;
  showAddBookToRecommendations: boolean = false;
  newMembershipOffer: boolean = false;
  membershipOfferSliderPos: number = 1;
  showEditBusinessHours = false;
  businessHoursEdit: BusinessHoursEdit = new BusinessHoursEdit();
  newLocation: MapLocation = new MapLocation();
  editingLocation: boolean = false;
  locationSubmitLoading: boolean = false;
  locationRemoveLoading: boolean = false;
  newLocalLocation: boolean = false;

  constructor(
    private authService: AuthService,
    private libraryService: LibraryService,
    private bookService: BookService,
    private titleService: Title
  ) {
    this.library = new Library();
    this.libraryEdit = this.library;
    this.booksRecommendations = [];
    this.bannerUploadOptions = {
      url: HttpConfig.EndPoints.library.banner,
      title: 'UREDI BANER SLIKU',
      subtitle:
        'Za baner sliku se preporučuje slika u omjeru 1:3, sa minimalnom rezolucijom od 400x1200. Slika će biti srezana u zavisnosti od dimenzija monitora.',
      currentImage: this.library.bannerImage,
    };

    this.profileImgUploadOptions = {
      url: HttpConfig.EndPoints.library.profileImg,
      title: 'UREDI PROFILNU SLIKU',
      subtitle:
        'Za profilnu sliku se preporučuje slika u omjeru 1:1, sa minimalnom rezolucijom od 150x150.',
      currentImage: this.library.profileImage,
    };
  }

  ngOnInit() {
    this.authService.accountType.subscribe((accountType) => {
      if (accountType == AccountType.User) {
        window.location.replace('/');
      }
    });

    this.libraryService.librarianGetLibrary().subscribe((res) => {
      if (res.location) {
        this.newLocation.zoom = res.location.zoom;
      }
      this.library = res;
      this.titleService.setTitle(`${this.library.name} - eBiblioteka`);
      this.libraryEdit = this.library;
      this.bannerUploadOptions.currentImage = this.library.bannerImage;
      this.profileImgUploadOptions.currentImage = this.library.profileImage;
    });
    this.loadRecommendations();
  }

  loadRecommendations() {
    this.bookService.getRecommendations().subscribe((res) => {
      this.booksRecommendations = res;
    });
  }

  setShowAddBookToRecommendations(option: boolean) {
    this.showAddBookToRecommendations = option;
    this.loadRecommendations();
  }

  getRecommendationsId() {
    this.bookService.getRecommendationsID().subscribe((res) => {
      this.recommendationsID = res;
    });
  }

  getBannerPath() {
    if (this.library.bannerImage) {
      return createImgPath(this.library.bannerImage.path);
    } else {
      return '';
    }
  }

  getProfileImagePath() {
    if (this.library.profileImage) {
      return createImgPath(this.library.profileImage.path);
    } else {
      return '';
    }
  }

  setEditing(option: boolean) {
    this.editing = option;

    if (option === false) {
      this.libraryEdit = this.library;
    }
  }

  updateLibrary() {
    this.updateLoading = true;
    this.libraryService.updateLibrary(this.libraryEdit).subscribe(
      (res) => {
        alert('Uspjesno izmjenjen sadrzaj');
        this.library.name = res.name;
        this.library.about = res.about;
        this.editing = false;
        this.updateLoading = false;
      },
      (err) => {
        alert('Doslo je do greske');
        this.editing = false;
        this.updateLoading = false;
      }
    );
  }

  setShowBannerUpload(option: boolean) {
    this.showBannerUpload = option;
  }

  setShowProfileImgUpload(option: boolean) {
    this.showProfileImgUpload = option;
  }

  setBanner(banner: Image) {
    this.library.bannerImage = banner;
  }

  getMembershipOfferType(): typeof MembershipOfferType {
    return MembershipOfferType;
  }

  setNewMembershipOffer(option: boolean) {
    this.newMembershipOffer = option;

    if (option) {
      if (this.library.membershipOffers.length !== 0) {
        this.membershipOfferSliderPos = this.library.membershipOffers.length;
      }
    } else {
      this.membershipOfferSliderPos = 1;
    }
  }

  newMembershipOfferAdded(newOffer: MembershipOffer) {
    this.library.membershipOffers.push(newOffer);

    this.setNewMembershipOffer(false);
  }

  MembershipOfferUpdated(offer: MembershipOffer) {
    this.library.membershipOffers = this.library.membershipOffers.map(
      (element) => {
        if (element.id === offer.id) {
          return offer;
        } else {
          return element;
        }
      }
    );
  }

  MembershipOfferDeleted(offer: MembershipOffer) {
    this.library.membershipOffers = this.library.membershipOffers.filter(
      (element) => {
        return element.id !== offer.id;
      }
    );
  }

  setShowBusinessHoursEdit(option: boolean) {
    this.showEditBusinessHours = option;
  }

  NewBusinessHours() {
    this.businessHoursEdit = new BusinessHoursEdit();

    this.showEditBusinessHours = true;
  }

  EditBusinessHours(item: BusinessHours) {
    this.businessHoursEdit = convertToBusinessHoursEdit(item);

    this.showEditBusinessHours = true;
  }

  DeleteBusinessHours(item: BusinessHours) {
    if (
      confirm(
        `Da li ste sigurni da želite izbrisati informaciju o radnom vremenu '${item.title}'`
      )
    ) {
      this.libraryService.removeBusinessHours(item).subscribe(
        (res) => {
          this.library.businessHours = this.library.businessHours.filter(
            (hours) => {
              if (hours.id !== item.id) {
                return hours;
              } else {
                return;
              }
            }
          );
        },
        (err) => {
          alert('Greška pri brisanju informacije o radnom vremenu.');
        }
      );
    }
  }

  FilterAlteredBusinessHours(item: BusinessHours) {
    let flag = false;

    this.library.businessHours = this.library.businessHours.map((hours) => {
      if (hours.id === item.id) {
        flag = true;
        return item;
      } else {
        return hours;
      }
    });

    if (!flag) {
      this.library.businessHours.push(item);
    }
  }

  onMapClick(event: any) {
    if (!this.editingLocation) {
      return;
    }

    this.newLocation.latitude = event.coords.lat;
    this.newLocation.longitude = event.coords.lng;
  }

  setLocationEditing(option: boolean) {
    this.editingLocation = option;
  }

  addLocalNewLocation() {
    this.library.location = {
      description: '',
      latitude: 43.7,
      longitude: 17.664559,
      zoom: 6.5,
    };

    this.newLocation.zoom = 6.5;

    this.newLocalLocation = true;

    this.setLocationEditing(true);
  }

  removeLocalNewLocation() {
    this.library.location = undefined;

    this.newLocalLocation = false;

    this.setLocationEditing(false);
  }

  submitNewLocation() {
    this.locationSubmitLoading = true;

    this.libraryService.addLocation(this.newLocation).subscribe(
      (res) => {
        this.editingLocation = false;
        this.locationSubmitLoading = false;
        this.library.location = res;
        this.newLocation = new MapLocation();
        this.newLocation.zoom = res.zoom;
        this.newLocalLocation = false;
      },
      () => {
        alert('Došlo je do greške na serveru!');
      }
    );
  }

  removeLocation() {
    this.locationRemoveLoading = true;

    this.libraryService.removeLocation().subscribe(
      (res) => {
        this.editingLocation = false;
        this.locationRemoveLoading = false;
        this.library.location = undefined;
        this.newLocation = new MapLocation();
      },
      () => {
        alert('Došlo je do greške na serveru!');
      }
    );
  }
}
