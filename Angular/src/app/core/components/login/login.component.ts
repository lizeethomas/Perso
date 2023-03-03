import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, tap, take } from 'rxjs';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { HeaderService } from '../../services/header.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private fb:FormBuilder,
    private loginService:LoginService,
    private router: Router, 
    private headerService: HeaderService) {}

  mainForm!:FormGroup;
  loginCtrl!:FormControl;
  passwordCtrl!:FormControl;
  token$!:Observable<string>;
  token!: string;


  ngOnInit() {
    this.initFormControls();
    this.initMainForm();
    this.initObservables();
  }

  private initMainForm(): void {
    this.mainForm = this.fb.group({
      login: this.loginCtrl,
      password: this.passwordCtrl,
    });
  }

  private initFormControls() {
    this.loginCtrl = this.fb.control('', Validators.required);
    this.passwordCtrl = this.fb.control('', Validators.required);
  }

  private initObservables() {
    this.token$ = this.loginService.token$;
  }

  public submitForm(form:FormGroup) {
    this.loginService.login(form.value.login, form.value.password)
      .pipe(
        tap((data) => { 
          this.token = <string>localStorage.getItem("token");
          this.headerService.setStatus();
          if(this.token.length>1){
            this.router.navigateByUrl(`movies`);
          }
        })
      ).subscribe();
  } 

  public moveToHome() {
    localStorage.clear();
    this.headerService.setStatus();
    this.router.navigateByUrl(`movies`);
  }
}


// public submitForm(form:FormGroup) {
//   this.loginService.login(form.value.login, form.value.password);
//   this.token$.pipe(
//     take(1),
//     tap((data) => {
//       this.token = data;  
//       console.log(data);
//       this.headerService.setStatus(); 
//       if(this.token.length>1){
//         localStorage.setItem("token",this.token); 
//         localStorage.setItem("login",form.value.login);
//         this.headerService.setStatus(); 
//         this.router.navigateByUrl(`movies`);
//       };        
//     })
//   ).subscribe();
// } 