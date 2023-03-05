import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PokemonComponent } from './pokemon/pokemon.component';
import { PokemonService } from './pokemon.service';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    PokemonComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ], 
  providers: [
    PokemonService
  ], 
  exports: [
    PokemonComponent
  ]
})
export class PokemonModule { }
