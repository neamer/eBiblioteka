export class Notification {
  id: number;
  text: string;
  title: string;
  recipientID: number;
  senderID: number;

  constructor() {
    this.id = -1;
    this.text = ' ';
    this.title = ' ';
    this.recipientID = -1;
    this.senderID = -1;
  }
}

export class SendNotificationVM {
  text: string;
  title: string;
  recipientID: number;

  constructor() {
    this.text = ' ';
    this.title = ' ';
    this.recipientID = -1;
  }
}
export class NotificationListItemVM {
  id: number;
  text: string;
  title: string;
  date: Date;
  libraryName: string;

  constructor() {
    this.id = -1;
    this.text = ' ';
    this.title = ' ';
    this.date = new Date();
    this.libraryName = '';
  }
}
