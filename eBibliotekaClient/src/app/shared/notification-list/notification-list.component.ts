import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { LibraryService } from 'src/app/core/http/library.service';
import { NotificationListItemVM } from 'src/app/data/interfaces/Notification';
import {
  getFormattedDate,
  getFormattedDateTime,
} from '../../helpers/getFormattedDate';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css'],
})
export class NotificationListComponent implements OnInit {
  notifications: NotificationListItemVM[] = [];
  @Output() onActionComplete: EventEmitter<any> = new EventEmitter();
  constructor(private libraryService: LibraryService) {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
  }

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications() {
    this.libraryService.getNotificationsForUser().subscribe((res) => {
      this.notifications = res;
    });
  }
  closeOverlay() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onActionComplete.emit();
  }
  removeNotification(id: number) {
    this.libraryService.removeNotification(id).subscribe((res) => {
      this.loadNotifications();
    });
  }

  FormatDate(date: Date) {
    if (date) {
      return getFormattedDateTime(date);
    }
    return '';
  }
}
