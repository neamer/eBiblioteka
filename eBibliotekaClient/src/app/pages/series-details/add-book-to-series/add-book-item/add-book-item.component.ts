import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BookSearchVM } from 'src/app/data/interfaces/Book';
import { createImgPath } from 'src/app/helpers/CreateImgPath';

@Component({
  selector: 'app-add-book-item',
  templateUrl: './add-book-item.component.html',
  styleUrls: ['./add-book-item.component.css'],
})
export class AddBookItemComponent implements OnInit {
  @Input() book: BookSearchVM = new BookSearchVM();
  @Input() seriesID: number;
  @Input() loading: boolean = false;
  @Output() addBook: EventEmitter<any> = new EventEmitter();
  constructor() {
    this.seriesID = -1;
  }

  ngOnInit(): void {}
  getFullImagePath(path: string) {
    if (path) {
      return createImgPath(path);
    }
    return '';
  }
  selectBook() {
    this.addBook.emit(this.book);
  }
}
