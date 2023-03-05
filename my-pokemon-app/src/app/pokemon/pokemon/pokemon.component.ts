import { Component, OnInit } from '@angular/core';
import { Pokemon } from '../models/pokemon';
import { PokemonService } from '../pokemon.service';
import { Observable, tap, delay, take, switchMap } from 'rxjs';
import { Url } from '../models/url';

@Component({
  selector: 'app-pokemon',
  templateUrl: './pokemon.component.html',
  styleUrls: ['./pokemon.component.scss']
})
export class PokemonComponent implements OnInit {

  pokemon$!:Observable<Pokemon>;
  url$!:Observable<Url>;
  pokemon!:Pokemon;

  constructor(private pokemonService: PokemonService) { }

   ngOnInit(): void {
      this.initObservable();
      this.getPokemon();
      this.getImg();
   }

  initObservable() {
    this.pokemon$ = this.pokemonService.pokemon$;
    this.url$ = this.pokemonService.url$;
  }

  getPokemon() {
    this.pokemonService.getRandomPokemon();
  }

  getImg() {
    this.pokemon$.subscribe({
      next: (value:Pokemon) => {
        this.pokemon = value;
        this.pokemonService.getImgUrl(this.pokemon.name);
      }
    })
  }
}
