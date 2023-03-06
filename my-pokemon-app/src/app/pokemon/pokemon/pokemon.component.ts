import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Pokemon } from '../models/pokemon';
import { PokemonService } from '../pokemon.service';
import { Observable, tap, delay, take, switchMap } from 'rxjs';
import { Url } from '../models/url';
import { Bitmap } from '../models/bitmap';

@Component({
  selector: 'app-pokemon',
  templateUrl: './pokemon.component.html',
  styleUrls: ['./pokemon.component.scss']
})
export class PokemonComponent implements OnInit, AfterViewInit {

  @ViewChild('myCanvas', {static: true}) canvasRef?: ElementRef;

  pokemon$!:Observable<Pokemon>;
  url$!:Observable<Url>;
  image$!:Observable<Bitmap>;
  pokemon!:Pokemon;
  url!:string;

  constructor(private pokemonService: PokemonService) { }

   ngOnInit(): void {
      this.initObservable();
      this.getPokemon();
      this.getImg();
   }

   ngAfterViewInit(): void {
      this.getCropImg(100);
   }

  initObservable() {
    this.pokemon$ = this.pokemonService.pokemon$;
    this.url$ = this.pokemonService.url$;
    this.image$ = this.pokemonService.image$;
  }

  getPokemon() {
    this.pokemonService.getRandomPokemon();
  }

  getImg() {
    this.pokemon$.subscribe({
      next: (value:Pokemon) => {
        this.pokemon = value;
        this.pokemonService.getImgUrl(this.pokemon.name);
        this.pokemonService.getBitmap(this.pokemon.name);
      }
    })
  }

  getCropImg(size:number) {
    this.url$.subscribe({
      next: (value:Url) => {
        this.url = value.url;
        this.url = this.pokemonService.cropImg(this.url, size);
        console.log(this.url);
      }
    })
  }
 
}
