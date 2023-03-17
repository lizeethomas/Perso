import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PokemonComponent } from './pokemon/pokemon.component';
import { PokemonService } from './pokemon.service';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../shared/material/material.module';


@NgModule({
  declarations: [
    PokemonComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    MaterialModule,
  ], 
  providers: [
    PokemonService
  ], 
  exports: [
    PokemonComponent
  ]
})
export class PokemonModule { }
