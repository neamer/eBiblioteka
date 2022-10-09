import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth/auth.service';
import { LibraryService } from 'src/app/core/http/library.service';
import {
  Library,
  LibraryStatsVM,
  LibraryUpdateVM,
} from 'src/app/data/interfaces/Library';
import { HttpConfig } from '../../../configs/HttpConfig';
import { ImageUploadOptions } from '../../../data/types/ImageUploadOptions';
import { createImgPath } from '../../../helpers/CreateImgPath';
import { Image } from '../../../data/interfaces/Image';
import { MembershipOffer } from '../../../data/interfaces/MembershipOffer';
import {
  BusinessHours,
  BusinessHoursEdit,
  convertToBusinessHoursEdit,
} from 'src/app/data/interfaces/BusinessHours';
import { MapLocation } from '../../../data/interfaces/MapLocation';
import { BookSearchVM } from 'src/app/data/interfaces/Book';
import { BookService } from 'src/app/core/http/book.service';
import { MembershipOfferType } from '../../library-profile/membership-offer/MembershipOfferType';
import { BookSuggestionListVM } from '../../../data/interfaces/BookSuggestion';

@Component({
  selector: 'app-homepage-librarian',
  templateUrl: './homepage-librarian.component.html',
  styleUrls: [
    './homepage-librarian.component.css',
    '../../library-detail/library-detail.component.css',
  ],
})
export class HomepageLibrarianComponent implements OnInit {
  showAuthors: boolean = false;
  showAddBook: boolean = false;
  showSeries: boolean = false;

  suggestions: BookSuggestionListVM[] = [];
  stats: LibraryStatsVM = new LibraryStatsVM();

  showSuggestionsList: boolean = false;

  constructor(
    private authService: AuthService,
    private libraryService: LibraryService
  ) {}

  ngOnInit() {
    this.libraryService.getBookSuggestionsHomepage().subscribe((res) => {
      this.suggestions = res;
    });

    this.libraryService.getStatsHomepage().subscribe((res) => {
      this.stats = res;
    });
  }

  setShowAuthors(option: boolean) {
    this.showAuthors = option;
  }

  setShowBook(option: boolean) {
    this.showAddBook = option;
  }

  setShowSeries(option: boolean) {
    this.showSeries = option;
  }

  setShowSuggestionsList(option: boolean) {
    this.showSuggestionsList = option;
  }
}
