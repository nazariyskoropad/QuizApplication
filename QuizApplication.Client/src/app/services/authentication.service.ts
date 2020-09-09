import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { Admin } from '../models/admin';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentAdminSubject: BehaviorSubject<Admin>;
    public currentAdmin: Observable<Admin>;

    constructor(private http: HttpClient) {
        this.currentAdminSubject = new BehaviorSubject<Admin>(JSON.parse(localStorage.getItem('currentAdmin')));
        this.currentAdmin = this.currentAdminSubject.asObservable();
    }

    public get currentAdminValue(): Admin {
        return this.currentAdminSubject.value;
    }

    login(email: string, password: string) {
        return this.http.post<any>(`${environment.apiUrl}/account/login`, { email, password }, { withCredentials: true })
            .pipe(map(Admin => {
                localStorage.setItem('currentAdmin', JSON.stringify(Admin));
                this.currentAdminSubject.next(Admin);
                this.startRefreshTokenTimer();
                return Admin;
            }));
    }

    logout() {
        this.http.post<any>(`${environment.apiUrl}/account/revoke-token`, {}, { withCredentials: true });
        this.stopRefreshTokenTimer();
        localStorage.removeItem('currentAdmin');
        this.currentAdminSubject.next(null);
    }

    refreshToken() {
        return this.http.post<any>(`${environment.apiUrl}/account/refresh-token`, {}, { withCredentials: true })
            .pipe(map((user) => {
                this.currentAdminSubject.next(user);
                this.startRefreshTokenTimer();
                return user;
            }));
    }

    private refreshTokenTimeout;

    private startRefreshTokenTimer() {
        const jwtToken = JSON.parse(atob(this.currentAdminValue.jwtToken.split('.')[1]));

        // set a timeout to refresh the token a minute before it expires
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (60 * 1000);
        this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }
}