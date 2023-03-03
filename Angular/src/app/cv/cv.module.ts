import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CvRoutingModule } from './cv-routing.module';
import { CvComponent } from './components/cv/cv.component';
import { ItemComponent } from './components/item/item.component';
import { CvService } from './services/cv.service';
import { SharedModule } from '../shared/material/shared.module';
import { MaterialModule } from '../shared/material/material.module';
import { CoreModule } from '../core/core.module';


@NgModule({
  declarations: [
    CvComponent,
    ItemComponent, 
  ],
  imports: [
    CommonModule,
    CvRoutingModule,
    CoreModule, 
    SharedModule,
    MaterialModule,
  ], 
  exports: [

  ], 
  providers: [
    CvService
  ]
})
export class CVModule { }
