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
  
            // if ([401, 403].includes(err.status) && this.authenticationService.currentAdminValue) {
            //     // auto logout if 401 or 403 response returned from api
            //     this.authenticationService.logout();
            // }
  
            else 
                console.log(request, err, err.error);

            return throwError(err);
        }))
    }
}