import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, tap, of } from 'rxjs';
import { environment } from 'src/environments/environments';
import { Login } from '../models/Login';

@Injectable()

export class LoginService {

    constructor(private http: HttpClient) {

    }

    token$!:Observable<string>;
    token!:string;
    expire!:Date;
    logInfo!:Login;

    login(login:string, password: string) : Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type':'application/json',
        });
        return this.http.post(`${environment.apiUrl}/login/`,
        {
            login: login, 
            password: password
        },{   
            headers: headers,
            responseType:"json"
        }).pipe(
            tap((data) => {
                this.logInfo = <Login>data;
                if(this.logInfo.token.length>1){
                    let date:string = this.logInfo.expire.toString();
                    localStorage.setItem("username",this.logInfo.username); 
                    localStorage.setItem("email",this.logInfo.email); 
                    localStorage.setItem("role",this.logInfo.role); 
                    localStorage.setItem("token",this.logInfo.token); 
                    localStorage.setItem("expire",date);
                }
            })
        )
    }
}



    // private _token$ = new BehaviorSubject<string>("");

    // get token$() {
    //   return this._token$.asObservable();
    // }