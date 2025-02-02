import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth/auth.service';
import { AuthRepository } from '../../repositories/auth.repository';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private authRepository: AuthRepository,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // 🔹 Handle Unauthorized (Token Expired or Invalid)
          this.snackBar.open('Session expired. Please log in again.', 'Close', { duration: 3000 });
          this.authRepository.logout(); // Clear session
          this.router.navigate(['/login']);
        } else if (error.status === 403) {
          // 🔹 Handle Forbidden (Access Denied)
          this.snackBar.open('You do not have permission to access this resource.', 'Close', { duration: 3000 });
        } else if (error.status === 500) {
          // 🔹 Handle Internal Server Error
          this.snackBar.open('Server error. Please try again later.', 'Close', { duration: 3000 });
        } else {
          // 🔹 Handle Other Errors
          this.snackBar.open(`Error: ${error.message}`, 'Close', { duration: 3000 });
        }

        return throwError(() => error);
      })
    );
  }
}
