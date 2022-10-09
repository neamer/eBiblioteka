import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-helper',
  templateUrl: './helper.component.html',
  styleUrls: ['./helper.component.css'],
})
export class HelperComponent implements OnInit {
  @Output() onClose: EventEmitter<null> = new EventEmitter();
  section: number = 1;

  constructor() {}

  ngOnInit(): void {
    document.getElementsByTagName('body')[0].classList.add('stop-scrolling');
  }

  close() {
    document.getElementsByTagName('body')[0].classList.remove('stop-scrolling');
    this.onClose.emit();
  }

  getScrollPos() {
    return window.scrollY;
  }

  setSection(option: number) {
    this.section = option;
  }
}
