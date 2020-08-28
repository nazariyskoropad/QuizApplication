import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService,
                private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            if (err.status === 200) {
                return throwError(err);
            }
  
            else if (err.status === 0) {
                alert('Server is not responding');
            }
  
            else if (err.status === 401) {
                this.authenticationService.logout();
                this.router.navigate(['/login']);
            }
  
            else 
                console.log(request, err, err.error);

            return throwError(err);
        }))
    }
}