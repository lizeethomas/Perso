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
  shadow$!:Observable<Url>;
  image$!:Observable<Game>;
  pokemon!:Pokemon;
  mainForm!:FormGroup;
  guessCtrl!:FormControl;

  type1!:string;
  type2!:string;

  shadowHintToggle:boolean = false;
  shadowHint:boolean = false;

  score!:number;
  try!:string;
  nbTry:number = 0;

  gameStatus!:boolean;

  constructor(private pokemonService: PokemonService, private fb:FormBuilder) { }

   ngOnInit(): void {
      this.gameStatus = false;
      this.score = 100;
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
    this.shadow$ = this.pokemonService.shadow$;
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
        this.type1 = "../../assets/types/Miniature_Type_" + value.type1 + "_EV.png";
        if (value.type2 !== value.type1 && value.type2 !== null && value.type2 !== undefined) {
          this.type2 = "../../assets/types/Miniature_Type_" + value.type2 + "_EV.png";
        }
        this.pokemonService.getImgUrl(this.pokemon.name);
        this.pokemonService.getShadow(this.pokemon.name);
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
    if (this.testWin(this.try, this.pokemon.name)) {
      this.gameStatus = true;
      console.log(this.pokemon.name)
    }
    else {
      this.initFormControl();
      let val = this.score - 5;
      this.verifScore(val);
    }
    this.nbTry++;
  } 

  testWin(str1:string, str2:string) : boolean {
    const v1 = str1.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    const v2 = str2.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    return v1.localeCompare(v2, "en", { sensitivity: "base" }) === 0;
  }

  giveUp() {
    event?.preventDefault();
    this.nbTry=0;
    this.score=0;
    this.gameStatus = true;
  }

  toggleImage(event:any) {
    this.shadowHintToggle = !this.shadowHintToggle;
  }

  activeShadowHint() {
    this.shadowHint = true;
    this.shadowHintToggle = !this.shadowHintToggle;
    let val = this.score - 50;
    this.verifScore(val);
  }

  verifScore(val:number) {
    if (val < 0) {this.score = 0;}
    else (this.score = val);
  }
}
