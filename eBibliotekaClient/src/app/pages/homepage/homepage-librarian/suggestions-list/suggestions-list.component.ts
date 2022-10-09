import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { LibraryService } from '../../../../core/http/library.service';
import { BookSuggestionListVM } from '../../../../data/interfaces/BookSuggestion';
import { getFormattedDateTime } from '../../../../helpers/getFormattedDate';

@Component({
  selector: 'app-suggestions-list',
  templateUrl: './suggestions-list.component.html',
  styleUrls: ['./suggestions-list.component.css'],
})
export class SuggestionsListComponent implements OnInit {
  items: BookSuggestionListVM[] = [];
  @Output() onActionComplete: EventEmitter<any> = new EventEmitter();
  constructor(private libraryService: LibraryService) {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.libraryService.getBookSuggestions().subscribe((res) => {
      this.items = res;
    });
  }
  closeOverlay() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onActionComplete.emit();
  }
  removeSuggestion(id: number) {
    console.log(id);
    this.libraryService.removeBookSuggestion(id).subscribe((res) => {
      this.loadData();
    });
  }

  FormatDate(date: Date) {
    if (date) {
      return getFormattedDateTime(date);
    }
    return '';
  }
}
