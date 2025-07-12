import { AddressModel } from "src/app/shared/models/address.model";
import { DepartmentModel, DesignationModel } from "src/app/admin/company/models";
import { FileDetailModel } from "src/app/employee/profile/models";

export class EmployeeModel {
  id: number;
  name: string;
  firstName: string;
  lastName: string;
  employeeName: string;
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
  companyId: number;
  probationPeriod: number;
  workExperience: string;
  designationId: number;
  designationName: string;
  jobTitle: string;
  departmentId: number;
  departmentName:string;
  subDepartment: string;
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
  currentAddress: AddressModel;
  permanentAddress: AddressModel;
  department: DepartmentModel;
  designation: DesignationModel;
  code: string
  exitDate: string;
  noticePeriod: number;
  note: string;
  exitType: number;
  startedFrom: string;
  roleId: number;
  imageUrl:string;
  imageDetails:FileDetailModel;
  
  constructor() {
    this.currentAddress = new AddressModel();
    this.permanentAddress = new AddressModel();
    this.department = new DepartmentModel();
    this.designation = new DesignationModel();
    this.noticePeriod = 0;
    this.imageDetails = new FileDetailModel();
  }
}