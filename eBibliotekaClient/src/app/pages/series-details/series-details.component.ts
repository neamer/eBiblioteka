import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/auth/auth.service';
import { BookService } from 'src/app/core/http/book.service';
import { BookSearchVM } from 'src/app/data/interfaces/Book';
import { PagedList } from 'src/app/data/types/PagedList';

@Component({
  selector: 'app-series-details',
  templateUrl: './series-details.component.html',
  styleUrls: ['./series-details.component.css'],
})
export class SeriesDetailsComponent implements OnInit {
  SeriesId: number;
  page: number = 1;
  books: BookSearchVM[];
  showAdd: boolean = false;
  isAuth: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private authService: AuthService,
    private titleService: Title
  ) {
    this.SeriesId = -1;
    this.books = [];
  }

  ngOnInit(): void {
    this.titleService.setTitle('Serijal - eBiblioteka');
    this.SeriesId = parseInt(this.route.snapshot.params['id']);
    this.authService.SeriesAuthCheck(this.SeriesId).subscribe((res) => {
      this.isAuth = res;
    });
    this.loadPage();
  }
  loadPage() {
    this.bookService.getBooksBySeries(this.SeriesId).subscribe((res) => {
      this.books = res;
    });
  }
  setShowAdd(option: boolean) {
    this.showAdd = option;
  }

  handleAddBook() {
    this.setShowAdd(false);

    this.loadPage();
  }
}
