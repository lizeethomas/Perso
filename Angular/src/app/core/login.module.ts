import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/material/shared.module';
import { MaterialModule } from '../shared/material/material.module';
import { LoginService } from './services/login.service';
import { LoginInterceptor } from './interceptors/login.interceptors'
import { ReactiveFormsModule } from '@angular/forms';
import  { httpInterceptorProviders } from './interceptors/index';

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule, 
    RouterModule,
    SharedModule,
    MaterialModule,
    ReactiveFormsModule,
  ],
  exports: [
    LoginComponent,
  ],  
  providers: [
    LoginService, 
    LoginInterceptor,
    httpInterceptorProviders, 
  ],
})
export class LoginModule {

  constructor() {

  }

}
