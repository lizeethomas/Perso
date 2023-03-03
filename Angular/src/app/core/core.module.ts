import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';

import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from '../shared/material/material.module';
import { httpInterceptorProviders } from './interceptors';
import { HeaderService } from './services/header.service';



@NgModule({
  declarations: [
    HeaderComponent
  ],
  imports: [
    CommonModule, 
    RouterModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
  ],
  exports: [
    HeaderComponent,
  ], 
  providers: [
    httpInterceptorProviders, 
    HeaderService,
  ]
})
export class CoreModule { }
