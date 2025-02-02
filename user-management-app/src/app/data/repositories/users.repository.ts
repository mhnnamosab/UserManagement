import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginResponse } from '../models/loginResponse.model';
import { User } from '../../core/domain/models/user.model';
interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
  }
@Injectable({
  providedIn: 'root',
})
export class UsersRepository {
  private apiUrl = environment.apiUrl + '/user'; // Use environment.apiUrl

  constructor(private http: HttpClient) { }

  // Get Users with Pagination and Search
  getUsersPaginated(page: number, pageSize: number, searchQuery: string,sortBy: string,ascending: boolean): Observable<PaginatedResponse<User>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('sortBy', sortBy.toString())
      .set('ascending', ascending.toString())
      .set('search', searchQuery);  // Pass the search query here

    return this.http.get<PaginatedResponse<User>>(this.apiUrl, { params });
  }
    
    getUsers(): Observable<User[]> {
      return this.http
        .get<any>(`${this.apiUrl}`)
        .pipe(
          catchError((error) => {
            console.error('get users error:', error);
            throw error;
          })
        );
    }


  // Add User
  addUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  // Edit User
  editUser(id: number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }

  // Delete User
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
    
}