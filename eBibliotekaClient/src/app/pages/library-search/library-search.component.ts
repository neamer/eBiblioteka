import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { LibraryService } from 'src/app/core/http/library.service';
import { LibraryListVM } from 'src/app/data/interfaces/Library';
import { PagedList } from '../../data/types/PagedList';
import { LibrarySearchType } from './LibrarySearchType';

@Component({
  selector: 'app-library-search',
  templateUrl: './library-search.component.html',
  styleUrls: ['./library-search.component.css'],
})
export class LibrarySearchComponent implements OnInit {
  searchType: LibrarySearchType = LibrarySearchType.List;

  constructor(private titleService: Title) {}

  ngOnInit(): void {
    this.titleService.setTitle('Pretraga biblioteka - eBiblioteka');
  }

  getSearchType(): typeof LibrarySearchType {
    return LibrarySearchType;
  }

  setSearchType(type: LibrarySearchType) {
    this.searchType = type;
  }
}
