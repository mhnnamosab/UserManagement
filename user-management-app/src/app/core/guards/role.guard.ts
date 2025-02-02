import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';
@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // Check if the user is authenticated
    if (this.authService.isAuthenticated()) {
      // Get the required roles from route data
      const requiredRoles = next.data['roles'];
      const currentUser = this.authService.currentUserValue;

      // Check if the current user has the required roles
      if (currentUser && requiredRoles.some((role: string) => currentUser.role == role)) {
        return true;
      } else {
        // User does not have the required role, redirect to a not-authorized page or home
        this.router.navigate(['/not-authorized']);
        return false;
      }
    } else {
      // If the user is not authenticated, redirect to login
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }
  }
}
