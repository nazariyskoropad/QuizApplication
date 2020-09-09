import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TestDetailed } from '../models/test-detailed';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
  })
export class TestService {

    public constructor(private httpClient: HttpClient) { }

    public getTests() {
        return this.httpClient.get<TestDetailed[]>(`${environment.apiUrl}/test`)
            .pipe(map((tests: TestDetailed[]) => {
                return tests;
        },
        error => {
            console.log(error);
        }));
    }

    public getTest(id: number) {
        return this.httpClient.get<TestDetailed>(`${environment.apiUrl}/test/${id}`)
            .pipe(map((test: TestDetailed) => {
                return test;
        },
        error => {
            console.log(error);
        }));
    }

    public deleteTest(id: number) {
        return this.httpClient.delete(`${environment.apiUrl}/test/${id}`)
            .pipe(map((test: TestDetailed) => {
                return test;
        }, 
            error => {
            console.log(error);
        }))
    }

    public addTest(test: TestDetailed) {
        return this.httpClient
        .post<any>(`${environment.apiUrl}/test`, test)
        .pipe(map(( data: any) => {
            return data;
          },
          error => {
            console.log(error);
          }));
    }

    public editTest(test: TestDetailed) {
        return this.httpClient
            .put<any>(`${environment.apiUrl}/test/${test.id}`, test)
            .pipe(map(( data: any) => {
                return data;
              },
              error => {
                console.log(error);
              }));
    }
}
