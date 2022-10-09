import { BusinessHours } from './BusinessHours';
import { Image } from './Image';
import { MapLocation } from './MapLocation';
import { Membership } from './Membership';
import { MembershipOffer } from './MembershipOffer';

export class Library {
  id: number;
  name: string;
  about: string;
  profileImage?: Image;
  bannerImage?: Image;
  membershipOffers: MembershipOffer[];
  businessHours: BusinessHours[];
  membership: Membership;
  location?: MapLocation;
  noOfBooks: number;
  noOfMembers: number;

  /**
   *
   */
  constructor() {
    this.id = -1;
    this.name = 'Učitavanje...';
    this.about = 'Učitavanje...';
    this.membershipOffers = [];
    this.businessHours = [];
    this.membership = new Membership();
    this.location = undefined;
    this.noOfBooks = 0;
    this.noOfMembers = 0;
  }
}

export interface LibraryUpdateVM {
  name: string;
  about: string;
}

export interface LibraryListVM {
  id: number;
  name: string;
  about: string;
  profileImage: string;
  bannerImage: string;
  isMember: boolean;
  location: MapLocation;
  noOfBooks: number;
  noOfMembers: number;
}

export class UserLibraryVM {
  id: number;
  name: string;
  about: string;
  profileImage?: string;
  bannerImage?: string;
  noOfBooks: number;
  noOfMembers: number;

  /**
   *
   */
  constructor() {
    this.id = -1;
    this.name = 'Učitavanje...';
    this.about = 'Učitavanje...';
    this.profileImage = '';
    this.bannerImage = '';
    this.noOfBooks = 0;
    this.noOfMembers = 0;
  }
}

export class LibraryStatsVM {
  noOfBooks: number;
  noOfMembers: number;

  /**
   *
   */
  constructor() {
    this.noOfBooks = 0;
    this.noOfMembers = 0;
  }
}
