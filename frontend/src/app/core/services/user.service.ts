import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { PaginatedResponse } from '../models/paginated-response.model';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { BaseResponse } from '../models/base-response.model';
import { Permission } from '../models/permission.model';

@Injectable({
	providedIn: 'root'
})
export class UserService {

	constructor(private _http: HttpClient) { }

    public getAllUsers(page: number, perPage: number, orderBy: string, orderType: string, filterBy: string, filter: string): Observable<BaseResponse<PaginatedResponse<User>>>{
        const queryParams = new HttpParams({
            fromObject: {page, perPage, orderBy, orderType, filterBy, filter}
        });

        return this._http.get<BaseResponse<PaginatedResponse<User>>>(`${environment.baseUrl}/api/Users/GetAllUsers`, { params: queryParams });
    }

    public deleteUser(userId: number): Observable<boolean> {
        return this._http.delete(`${environment.baseUrl}/api/Users/DeleteUser/${userId}`).pipe(
            map(_ => true),
            catchError(() => of(false))
        );
    }

    public createUser(user: User): Observable<BaseResponse<User>> {
        return this._http.post<BaseResponse<User>>(`${environment.baseUrl}/api/Users/CreateUser`, user);
    }

    public getUserById(userId: number): Observable<BaseResponse<User>> {
        return this._http.get<BaseResponse<User>>(`${environment.baseUrl}/api/Users/GetUserById/${userId}`);
    }

    public updateUser(userId: number, user: User): Observable<BaseResponse<User>> {
        return this._http.put<BaseResponse<User>>(`${environment.baseUrl}/api/Users/UpdateUser/${userId}`, user);
    }

    public getAllPermissions(): Observable<Permission[]> {
        return this._http.get<BaseResponse<Permission[]>>(`${environment.baseUrl}/api/Permissions/GetAllPermissions`).pipe(
            map(response => {
                if(!response.status) {
                    return [];
                }

                return response.data as Permission[];
            }),
            catchError(() => of([]))
        );
    }

    public getUserPermissions(userId: number): Observable<Permission[]> {
        return this._http.get<BaseResponse<Permission[]>>(`${environment.baseUrl}/api/Permissions/GetUserPermissions/${userId}`).pipe(
            map(response => {
                if(!response.status) {
                    return [];
                }

                return response.data as Permission[];
            }),
            catchError(() => of([]))
        );
    }

    public assignUserPermission(userId: number, permissionId: number): Observable<boolean> {
        return this._http.get(`${environment.baseUrl}/api/Permissions/AssignUserPermission/${userId}/${permissionId}`).pipe(
            map(_ => true),
            catchError(() => of(false))
        );
    }

    public revokeUserPermission(userId: number, permissionId: number): Observable<boolean> {
        return this._http.get(`${environment.baseUrl}/api/Permissions/RevokeUserPermission/${userId}/${permissionId}`).pipe(
            map(_ => true),
            catchError(() => of(false))
        );
    }
}
