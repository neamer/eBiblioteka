import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { BookService } from 'src/app/core/http/book.service';
import { LibraryService } from 'src/app/core/http/library.service';
import { BookSearchVM } from 'src/app/data/interfaces/Book';
import { Library, UserLibraryVM } from 'src/app/data/interfaces/Library';
import { createImgPath } from 'src/app/helpers/CreateImgPath';
import { MembershipOffer } from '../../data/interfaces/MembershipOffer';

@Component({
  selector: 'app-library-detail',
  templateUrl: './library-detail.component.html',
  styleUrls: ['./library-detail.component.css'],
})
export class LibraryDetailComponent implements OnInit {
  id: number;
  library: Library;
  libraryVM: UserLibraryVM = new UserLibraryVM();
  booksRecommendations: BookSearchVM[] = [];
  showMembershipForm: boolean = false;
  selectedOffer: MembershipOffer = new MembershipOffer();

  showAddSuggestion: boolean = false;

  subscriptionOfferId: number = -1;

  constructor(
    private route: ActivatedRoute,
    private libraryService: LibraryService,
    private bookService: BookService,
    private titleService: Title
  ) {
    this.id = -1;
    this.library = new Library();
  }

  ngOnInit() {
    this.id = parseInt(<string>this.route.snapshot.paramMap.get('id'));
    this.libraryService.getLibrary(this.id).subscribe((res) => {
      this.library = res;
      this.titleService.setTitle(`${this.library.name} - eBiblioteka`);
      this.libraryVM = {
        id: this.library.id,
        name: this.library.name,
        about: this.library.about,
        noOfBooks: this.library.noOfBooks,
        noOfMembers: this.library.noOfMembers,
        profileImage: this.library.profileImage?.path,
        bannerImage: this.library.bannerImage?.path,
      };
      if (this.library.membership) {
        this.subscriptionOfferId = this.library.membership.membershipOffer.id;
      }
    });
    this.loadRecommendations();
  }
  loadRecommendations() {
    this.bookService.getRecommendationsUser(this.id).subscribe((res) => {
      this.booksRecommendations = res;
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

  openMembershipForm(offer: MembershipOffer) {
    this.selectedOffer = offer;
    this.setShowMembershipForm(true);
  }

  setShowMembershipForm(option: boolean) {
    this.showMembershipForm = option;
  }

  formatExpirationDate() {
    let date = new Date(this.library.membership.expirationDate);

    return date.toLocaleString('en-GB', { timeZone: 'UTC' });
  }

  setShowAddSuggestion(option: boolean) {
    this.showAddSuggestion = option;
  }
}
