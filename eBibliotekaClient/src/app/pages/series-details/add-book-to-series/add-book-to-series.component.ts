import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BookService } from 'src/app/core/http/book.service';
import { BookSearchVM } from 'src/app/data/interfaces/Book';

@Component({
  selector: 'app-add-book-to-series',
  templateUrl: './add-book-to-series.component.html',
  styleUrls: ['./add-book-to-series.component.css'],
})
export class AddBookToSeriesComponent implements OnInit {
  libraryID: number = -1;
  books: BookSearchVM[] = [];
  @Input() seriesID: number = -1;
  @Output() onActionComplete: EventEmitter<any> = new EventEmitter();
  @Output() onAddComplete: EventEmitter<any> = new EventEmitter();
  filter: string = '';
  @Input() isRecommendation = false;
  constructor(private bookService: BookService) {}

  ngOnInit(): void {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
    this.loadBooks();
  }

  getScrollPos() {
    return window.scrollY;
  }

  loadBooks() {
    this.bookService.searchLibrarianList(this.filter).subscribe(
      (res) => {
        this.books = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  addBookToSeries(book: BookSearchVM) {
    if (this.isRecommendation) {
      this.bookService.addRecommendation(book.id).subscribe((res) => {
        this.onAddComplete.emit();
        this.onActionComplete.emit();
        this.onComplete();
      });
    } else {
      this.bookService.addBookToSeries(book.id, this.seriesID).subscribe(
        (res) => {
          this.onAddComplete.emit();
          this.onComplete();
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }

  onComplete() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onActionComplete.emit();
  }
}
