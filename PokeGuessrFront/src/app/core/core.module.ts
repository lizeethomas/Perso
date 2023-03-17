import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { MaterialModule } from '../shared/material/material.module';
import { FooterComponent } from './footer/footer.component';



@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    CommonModule, 
    MaterialModule,
  ], 
  exports: [
    HeaderComponent,
  ]
})
export class CoreModule { }
