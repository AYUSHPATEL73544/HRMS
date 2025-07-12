import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { EmployeeModel } from "src/app/admin/directory/models/index";
import { FilterModel } from "src/app/shared/models";

@Injectable()
export class EmployeeService {

  private apiEndPoint = environment.apiBaseUrl + '/employee';

  constructor(private http: HttpClient) { }

  add(model: EmployeeModel): Observable<any> {
    return this.http.post(`${this.apiEndPoint}`, model);
  }

  pageList(model: FilterModel, employeeStatus: number): Observable<any> {
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/page-list?${queryString}&employeeStatus=${employeeStatus}`);
  }

  inactiveEmployeeList(model: FilterModel): Observable<any> {
    const queryString = model.toQueryString();
    return this.http.get(`${this.apiEndPoint}/inactive-employee-list?${queryString}`);
  }

  getEmployeesList(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/list`);
  }

  getEmployee(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/${id}`);
  }

  getEmployeesByDepartmentId(departmentId: number): Observable<any>
  {
    return this.http.get(`${this.apiEndPoint}/getByDepartmentId/${departmentId}`);
  }

  getEmployeesByDesignationId(designationId: number):Observable<any>{
    return this.http.get(`${this.apiEndPoint}/getByDesignationId/${designationId}`);
  }

  getSelectListItem(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/get-select-list-item`)
  }

  getManagerSelectListItem(): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/manager-list`)
  }

  getEmployeeSelectListItem(leaveRuleId: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/select-list-items/${leaveRuleId}`)
  }

  getSelectListItemByAttendanceRuleId(attendanceRuleId: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/select-list/${attendanceRuleId}`)
  }

  getCompanyEvent(month: number): Observable<any> {
    const queryString = `month=${month}`;
    return this.http.get(`${this.apiEndPoint}/calendar-event?${queryString}`);
  }

  update(model: EmployeeModel): Observable<any> {
    return this.http.put(`${this.apiEndPoint}`, model);
  }

  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiEndPoint}/${id}`);
  }

  getNameById(id: number): Observable<any> {
    return this.http.get(`${this.apiEndPoint}/name/${id}`);
  }
}