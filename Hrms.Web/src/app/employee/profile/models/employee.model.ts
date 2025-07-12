import { AddressModel } from "src/app/shared/models/address.model";
import { DepartmentModel, DesignationModel } from "src/app/employee/company/models";
import { FileDetailModel } from "../../job-recruitment/model";

export class
  EmployeeModel {
  id: number;
  name: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: number;
  bloodGroup: number;
  maritalStatus: number;
  email: string;
  alternateEmail: string;
  phone: number;
  alternatePhone: number;
  employeeID: string;
  dateOfJoining: string;
  dateOfLeaving: string;
  probationPeriod: number;
  workExperience: string;
  designationId: number;
  designationName: string;
  jobTitle: string;
  departmentId: number;
  departmentName:string;
  fromDate: string;
  toDate: string;
  employeeType: number;
  relationship: string;
  qualificationType: string;
  universityName: string;
  collegeName: string;
  courseType: string;
  courseName: string;
  stream: string;
  courseStartDate: string;
  courseEndDate: string;
  uploadedBy: string;
  status: number;
  bloodGroupName: string
  genderName: string;
  martialStatus: string;
  currentAddress: AddressModel;
  permanentAddress: AddressModel;
  department: DepartmentModel;
  designation: DesignationModel;
  code: string;
  exitDate: string;
  noticePeriod: string;
  note: string;
  exitType: number;
  employeeStatus: string;
  createdOn: string;
  type: string;
  imageDetails:FileDetailModel;
  imageFile:File;
  imageUrl:string;
  constructor() {
    this.currentAddress = new AddressModel();
    this.permanentAddress = new AddressModel();
    this.department = new DepartmentModel();
    this.designation = new DesignationModel();
    this.imageDetails= new FileDetailModel();
  }
}