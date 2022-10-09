import { Account } from './Account';

export class BookSuggestionAddVM {
  title: string;
  author: string;

  constructor() {
    this.title = '';
    this.author = '';
  }
}

export class BookSuggestionListVM {
  id: number;
  title: string;
  author: string;
  user: Account;

  constructor() {
    this.id = -1;
    this.title = '';
    this.author = '';
    this.user = new Account();
  }
}
