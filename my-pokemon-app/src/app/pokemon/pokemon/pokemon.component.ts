import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Pokemon } from '../models/pokemon';
import { PokemonService } from '../pokemon.service';
import { Observable, tap, delay, take, switchMap } from 'rxjs';
import { Url } from '../models/url';
import { Bitmap } from '../models/bitmap';
import { Setup } from '../models/setup';
import { Game } from '../models/game';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-pokemon',
  templateUrl: './pokemon.component.html',
  styleUrls: ['./pokemon.component.scss']
})
export class PokemonComponent implements OnInit, AfterViewInit {

  @ViewChild('myCanvas', {static: true}) canvasRef?: ElementRef;

  pokemon$!:Observable<Pokemon>;
  url$!:Observable<Url>;
  image$!:Observable<Game>;
  pokemon!:Pokemon;
  mainForm!:FormGroup;
  guessCtrl!:FormControl;

  try!:string;

  gameStatus!:boolean;

  constructor(private pokemonService: PokemonService, private fb:FormBuilder) { }

   ngOnInit(): void {
      this.gameStatus = false;
      this.initFormControl();
      this.initObservable();
      this.getPokemon();
      this.getImg();
   }

   ngAfterViewInit(): void {
      this.getGame(100);
   }

  initObservable() : void  {
    this.pokemon$ = this.pokemonService.pokemon$;
    this.url$ = this.pokemonService.url$;
    this.image$ = this.pokemonService.image$;
  }

  initFormControl() : void  {
    this.guessCtrl = this.fb.control("", Validators.required);
  }

  getPokemon() : void  {
    this.pokemonService.getRandomPokemon();
  }

  getImg() : void  {
    this.pokemon$.subscribe({
      next: (value:Pokemon) => {
        this.pokemon = value;
        this.pokemonService.getImgUrl(this.pokemon.name);
      }
    })
  }

  getGame(size:number) {
    let setup:Setup = new Setup();
    this.url$.pipe().subscribe({
      next: (url) => {
        setup.url = url.url;
        setup.size = size;
        this.pokemonService.getGame(setup);
      }
    });
  }

  onSubmit() : void {
    event?.preventDefault();
    this.try = this.guessCtrl.value;
    if (this.try == this.pokemon.name) {
      this.gameStatus = true;
      console.log(this.pokemon.name)
    }
    this.initFormControl();
  } 

  verifGame() {

  }
}
