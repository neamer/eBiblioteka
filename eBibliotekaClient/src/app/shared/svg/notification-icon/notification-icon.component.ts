import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-notification-icon',
  templateUrl: './notification-icon.component.html',
  styleUrls: ['./notification-icon.component.css'],
})
export class NotificationIconComponent implements OnInit {
  @Input() svgClass: string;
  constructor() {
    this.svgClass = '';
  }
  ngOnInit(): void {}
}
