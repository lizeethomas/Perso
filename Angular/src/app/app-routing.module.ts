import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path: 'login', loadChildren: () => import('./core/login-routing.module').then(m =>m.LoginRoutingModule)},
  {path: 'movies', loadChildren: () => import('./movies/movies-routing.module').then(m =>m.MoviesRoutingModule)},
  {path: 'map', loadChildren: () => import('./map/map-routing.module').then(m =>m.MapRoutingModule)},
  {path: 'CV', loadChildren: () => import('./cv/cv-routing.module').then(m =>m.CvRoutingModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
}) 
export class AppRoutingModule { }
