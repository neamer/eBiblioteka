import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletIconComponent } from './wallet-icon.component';

describe('WalletIconComponent', () => {
  let component: WalletIconComponent;
  let fixture: ComponentFixture<WalletIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WalletIconComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WalletIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
