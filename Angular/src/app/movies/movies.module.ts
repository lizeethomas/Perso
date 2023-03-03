import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MoviesRoutingModule } from './movies-routing.module';
import { MoviesComponent } from './components/movies/movies.component';
import { MoviesService } from './services/movies.service'
import { MaterialModule } from '../shared/material/material.module';

import { ReactiveFormsModule } from '@angular/forms';
import { MovieComponent } from './components/movie/movie.component';
import { MovieEditComponent } from './components/movie-edit/movie-edit.component';
import { SharedModule } from '../shared/material/shared.module';
import { CoreModule } from '../core/core.module';
import { LoginService } from '../core/services/login.service';


@NgModule({
  declarations: [
    MoviesComponent,
    MovieComponent,
    MovieEditComponent, 
  ],
  imports: [
    CommonModule,
    MoviesRoutingModule, 
    MaterialModule,
    ReactiveFormsModule,
    SharedModule,
    CoreModule,
  ], 
  providers: [
    MoviesService, 
    LoginService,
  ], 
  exports : [
    ReactiveFormsModule,
  ]
})
export class MoviesModule { }
