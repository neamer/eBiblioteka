import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookIconComponent } from './book-icon.component';

describe('BookIconComponent', () => {
  let component: BookIconComponent;
  let fixture: ComponentFixture<BookIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookIconComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
