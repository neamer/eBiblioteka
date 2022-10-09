import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BookService } from 'src/app/core/http/book.service';
import { SeriesListItem } from 'src/app/data/interfaces/Series';

@Component({
  selector: 'app-series-item-auth',
  templateUrl: './series-item-auth.component.html',
  styleUrls: ['./series-item-auth.component.css'],
})
export class SeriesItemAuthComponent implements OnInit {
  @Output() onDelete: EventEmitter<number> = new EventEmitter<number>();
  @Input() seriesItem: SeriesListItem;
  constructor(private bookService: BookService) {
    this.seriesItem = new SeriesListItem();
  }

  ngOnInit(): void {}

  deleteSeries() {
    this.onDelete.emit(this.seriesItem.id);
  }
}
