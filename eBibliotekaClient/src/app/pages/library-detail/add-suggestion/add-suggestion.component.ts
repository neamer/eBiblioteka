import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LibraryService } from '../../../core/http/library.service';
import { BookSuggestionAddVM } from '../../../data/interfaces/BookSuggestion';

@Component({
  selector: 'app-add-suggestion',
  templateUrl: './add-suggestion.component.html',
  styleUrls: [
    './add-suggestion.component.css',
    '../../member-search/member-list/send-notification/send-notification.component.css',
  ],
})
export class AddSuggestionComponent implements OnInit {
  data: BookSuggestionAddVM = new BookSuggestionAddVM();
  @Input() libraryID: number;
  @Output() onActionComplete = new EventEmitter();
  constructor(private libraryService: LibraryService) {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
    this.libraryID = -1;
  }

  getScrollPos() {
    return window.scrollY;
  }

  ngOnInit(): void {}

  submitSuggestion() {
    if (this.data.title.length < 2) {
      alert('Naslov mora imati vise od dva karaktera!');
    } else if (this.data.author.length < 2) {
      alert('Naziv autora mora imati vise od dva karaktera!');
    } else {
      this.libraryService
        .addBookSuggestion(this.libraryID, this.data)
        .subscribe((res) => {
          alert('Preporuka uspješno poslana');
          this.closeOverlay();
        });
    }
  }

  closeOverlay() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onActionComplete.emit();
  }
}
