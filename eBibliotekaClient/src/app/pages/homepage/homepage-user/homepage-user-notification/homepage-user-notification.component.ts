import { Component, OnInit } from '@angular/core';
import { LibraryService } from '../../../../core/http/library.service';
import { NotificationListItemVM } from '../../../../data/interfaces/Notification';

@Component({
  selector: 'app-homepage-user-notification',
  templateUrl: './homepage-user-notification.component.html',
  styleUrls: ['./homepage-user-notification.component.css'],
})
export class HomepageUserNotificationComponent implements OnInit {
  items: NotificationListItemVM[] = [];
  loading: boolean = false;

  constructor(private libraryService: LibraryService) {}

  ngOnInit(): void {
    this.loading = true;
    this.libraryService.getNotificationsForUser().subscribe(
      (res) => {
        this.items = res;
        this.loading = false;
      },
      (err) => {
        console.log(err);
        this.loading = false;
      }
    );
  }

  getFormattedContent(text: string) {
    if (text && text.length > 150) {
      return text.slice(0, 147) + '...';
    }
    return text;
  }

  FormatDate(rawDate: Date) {
    var date = new Date(rawDate);
    var month = '';

    switch (date.getMonth()) {
      case 0: {
        month = 'JAN';
        break;
      }
      case 1: {
        month = 'FEB';
        break;
      }
      case 2: {
        month = 'MAR';
        break;
      }
      case 3: {
        month = 'APR';
        break;
      }
      case 4: {
        month = 'MAJ';
        break;
      }
      case 5: {
        month = 'JUN';
        break;
      }
      case 6: {
        month = 'JUL';
        break;
      }
      case 7: {
        month = 'AUG';
        break;
      }
      case 8: {
        month = 'SEP';
        break;
      }
      case 9: {
        month = 'OKT';
        break;
      }
      case 10: {
        month = 'NOV';
        break;
      }
      case 11: {
        month = 'DEC';
        break;
      }
      default: {
        break;
      }
    }

    return `${date.getDate()}. ${month}`;
  }
}
