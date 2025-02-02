import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LocalStorageService } from '../../utils/localstorage/local-storage.service';
import { AUTH_TOKEN_KEY, CURRENT_USER_KEY } from '../../utils/localstorage/local-storage-key.constant';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private localStorageService:LocalStorageService) {
    // Initialize the currentUserSubject from localStorage (if JWT token exists)
    const storedUser = JSON.parse(this.localStorageService.getItem(CURRENT_USER_KEY) || '{}');
    this.currentUserSubject = new BehaviorSubject<any>(storedUser);
    this.currentUser = this.currentUserSubject.asObservable();
  }

  // Get the current logged-in user
  public get currentUserValue(): any {
    return this.currentUserSubject.value;
  }





  // Check if the user is authenticated (JWT token exists and is valid)
  isAuthenticated(): boolean {
    const token = this.localStorageService.getItem(AUTH_TOKEN_KEY);
    // Add logic to check if token is expired (if applicable)
    return !!token; // If token exists, return true (authenticated)
  }

  // Get the current JWT token
  getToken(): string | null {
    return this.localStorageService.getItem(AUTH_TOKEN_KEY);
  }

  // Check if the user has a specific role
  hasRole(role: number): boolean {
    const currentUser = this.currentUserValue;
    return currentUser && currentUser.role == role;
  }

  // Refresh the JWT token if necessary (e.g., by making an API call)

  setUser(user:any){
    this.currentUserSubject.next(user);
  }
}
