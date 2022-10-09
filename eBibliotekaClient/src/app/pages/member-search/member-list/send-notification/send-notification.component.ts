import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { LibraryService } from 'src/app/core/http/library.service';
import { SendNotificationVM } from 'src/app/data/interfaces/Notification';

@Component({
  selector: 'app-send-notification',
  templateUrl: './send-notification.component.html',
  styleUrls: ['./send-notification.component.css'],
})
export class SendNotificationComponent implements OnInit {
  data: SendNotificationVM = new SendNotificationVM();
  @Input() userID: number;
  @Output() onActionComplete = new EventEmitter();
  constructor(private libraryService: LibraryService) {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
    this.userID = -1;
  }

  getScrollPos() {
    return window.scrollY;
  }

  ngOnInit(): void {
    this.assignUserID();
  }
  assignUserID() {
    this.data.recipientID = this.userID;
  }
  closeOverlay() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onActionComplete.emit();
  }

  sendNotification() {
    if (this.data.title.length < 2) {
      alert('Naslov notifikacije mora imati vise od dva karaktera!');
    } else if (this.data.text.length < 2) {
      alert('Sadrzaj notifikacije mora imati vise od dva karaktera!');
    } else {
      this.libraryService.sendNotification(this.data).subscribe((res) => {
        console.log(res);
        this.closeOverlay();
      });
    }
  }
}
