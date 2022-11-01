import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginatedResponse } from '../models/paginated_response.model';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { BaseResponse } from '../models/base_response.model';

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

    public deleteUser(userId: number) {
        return this._http.delete(`${environment.baseUrl}/api/Users/DeleteUser/${userId}`);
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
}
