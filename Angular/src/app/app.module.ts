import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaterialModule } from './shared/material/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { MoviesModule } from './movies/movies.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/material/shared.module';
import { LoginModule } from './core/login.module';
import { MapModule } from './map/map.module';
import { MarkerService } from './map/marker.service';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from './map/map.service';
import { CVModule } from './cv/cv.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, 
    MaterialModule, 
    BrowserAnimationsModule,
    CoreModule,
    MoviesModule,
    ReactiveFormsModule,
    SharedModule,
    LoginModule,
    MapModule,
    HttpClientModule,
    CVModule,
  ],
  providers: [
    MarkerService,
    MapService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
