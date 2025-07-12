import { AddressModel } from "src/app/shared/models/address.model";

export class CompanyModel {
  id: number;
  registeredName: string;
  websiteUrl: string;
  brandName: string;
  email: string;
  phone: string;
  twitterUrl: string;
  facebookUrl: string;
  linkedInUrl: string;
  registeredOffice: AddressModel;
  corporateOffice: AddressModel;

  constructor() {
    this.registeredOffice = new AddressModel();
    this.corporateOffice = new AddressModel();
  }
}