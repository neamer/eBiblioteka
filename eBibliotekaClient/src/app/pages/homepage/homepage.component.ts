import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AuthService } from 'src/app/core/auth/auth.service';
import { AccountType } from 'src/app/data/enums/AccountType';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
})
export class HomepageComponent implements OnInit {
  accountType: AccountType | null;
  loading: boolean = false;

  constructor(private authService: AuthService, private titleService: Title) {
    this.loading = true;
    this.accountType = null;

    authService.accountType.subscribe((accountType) => {
      this.accountType = accountType;
      this.loading = false;
    });
    authService.setAccountType();
  }

  public get getAccountType(): typeof AccountType {
    return AccountType;
  }

  ngOnInit(): void {
    this.titleService.setTitle('Naslovna - eBiblioteka');
  }
}
