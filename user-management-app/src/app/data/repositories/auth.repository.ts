import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/services/auth/auth.service';
import { Router } from '@angular/router';
import { LocalStorageService } from '../../core/utils/localstorage/local-storage.service';
import { AUTH_TOKEN_KEY, CURRENT_USER_KEY } from '../../core/utils/localstorage/local-storage-key.constant';
import { LoginResponse } from '../models/loginResponse.model';

@Injectable({
  providedIn: 'root',
})
export class AuthRepository {
  private apiUrl = environment.apiUrl + '/auth'; // Use environment.apiUrl

  constructor(private http: HttpClient,private authService:AuthService, private router: Router,private localStorageService:LocalStorageService) { }


    // Login method
    login(credentials: { username: string; password: string }): Observable<any> {
      return this.http
        .post<LoginResponse>(`${this.apiUrl}/login`, credentials)
        .pipe(
          map((response) => {
            // Store the JWT token and user info in localStorage
            if (response && response.token) {
              this.localStorageService.setItem(CURRENT_USER_KEY,JSON.stringify(response.user))
              this.localStorageService.setItem(AUTH_TOKEN_KEY,response.token)
              this.authService.setUser(response.user)
            }
            return response;
          }),
          catchError((error) => {
            console.error('Login error:', error);
            throw error;
          })
        );
    }
      // Logout method
  logout(): void {
    // Remove user and token from localStorage and reset currentUserSubject
    this.localStorageService.removeItem(AUTH_TOKEN_KEY)
    this.localStorageService.removeItem(CURRENT_USER_KEY)
    this.authService.setUser(null)
    this.router.navigate(['/login']); // Redirect to the login page after logout
  }

    refreshToken(): Observable<any> {
      const token = this.authService.getToken();
      return this.http
        .post<any>(`${this.apiUrl}/refresh-token`, { token })
        .pipe(
          map((response) => {
            if (response && response.token) {
              this.localStorageService.setItem(AUTH_TOKEN_KEY,response.token)
            }
            return response;
          }),
          catchError((error) => {
            console.error('Token refresh error:', error);
            this.logout(); // Log out if token refresh fails
            throw error;
          })
        );
    }
}