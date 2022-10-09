import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomepageUserNotificationComponent } from './homepage-user-notification.component';

describe('HomepageUserNotificationComponent', () => {
  let component: HomepageUserNotificationComponent;
  let fixture: ComponentFixture<HomepageUserNotificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomepageUserNotificationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomepageUserNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
