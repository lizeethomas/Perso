import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MapRoutingModule } from './map-routing.module';
import { MapComponent } from './components/map/map.component';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { LocationComponent } from './location/location.component';
import { SharedModule } from '../shared/material/shared.module';
import { MaterialModule } from '../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MapService } from './map.service';
import { MarkerService } from './marker.service';

@NgModule({
  declarations: [
    MapComponent,
    LocationComponent,
  ],
  imports: [
    CommonModule,
    MapRoutingModule,
    LeafletModule,
    SharedModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
  ], 
  providers: [
    MapService,
    MarkerService,
  ],
  exports:  [
    ReactiveFormsModule,
  ]
})
export class MapModule { }
