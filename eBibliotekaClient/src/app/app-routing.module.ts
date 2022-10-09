import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './core/auth/guards/auth-guard.service';
import { BookDetailComponent } from './pages/book-detail/book-detail.component';
import { BookSearchComponent } from './pages/book-search/book-search.component';
import { HomepageComponent } from './pages/homepage/homepage.component';
import { LibraryDetailComponent } from './pages/library-detail/library-detail.component';
import { LibraryProfileComponent } from './pages/library-profile/library-profile.component';
import { LibrarySearchComponent } from './pages/library-search/library-search.component';
import { MemberSearchComponent } from './pages/member-search/member-search.component';
import { MyAccountComponent } from './pages/my-account/my-account.component';
import { RegisterLendingComponent } from './pages/register-lending/register-lending.component';
import { SeriesDetailsComponent } from './pages/series-details/series-details.component';

const routes: Routes = [
  { path: '', component: HomepageComponent },
  { path: 'login', component: HomepageComponent },
  { path: 'register', component: HomepageComponent },
  { path: 'biblioteke', component: LibrarySearchComponent },
  { path: 'biblioteka/:id', component: LibraryDetailComponent },
  {
    path: 'profil',
    component: MyAccountComponent,
    canActivate: [AuthGuardService],
    data: { title: 'eBiblioteka - Korisnički račun' },
  },
  {
    path: 'knjige',
    component: BookSearchComponent,
    canActivate: [AuthGuardService],
    data: { title: 'eBiblioteka - Knjige ' },
  },
  {
    path: 'biblioteka',
    component: LibraryProfileComponent,
    canActivate: [AuthGuardService],
    data: { title: 'eBiblioteka - Biblioteka' },
  },
  {
    path: 'knjige/:id',
    component: BookSearchComponent,
    data: { title: 'eBiblioteka - Knjige ' },
  },
  {
    path: 'knjiga/:id',
    component: BookDetailComponent,
    data: { title: 'eBiblioteka - Knjige ' },
  },
  {
    path: 'serijal/:id',
    component: SeriesDetailsComponent,
    data: { title: 'eBiblioteka - Serijal ' },
  },
  {
    path: 'korisnici',
    component: MemberSearchComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'evidencija-posudbe',
    component: RegisterLendingComponent,
    canActivate: [AuthGuardService],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload',
      scrollPositionRestoration: 'enabled',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
