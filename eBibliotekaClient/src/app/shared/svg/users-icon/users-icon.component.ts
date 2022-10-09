import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-users-icon',
  templateUrl: './users-icon.component.html',
  styleUrls: ['./users-icon.component.css'],
})
export class UsersIconComponent implements OnInit {
  @Input() svgClass: string;
  constructor() {
    this.svgClass = '';
  }
  ngOnInit(): void {}
}
