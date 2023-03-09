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
  url:Url = new Url();
  mainForm!:FormGroup;
  guessCtrl!:FormControl;

  type1!:string;
  type2!:string;
  unknownType:string = "../../assets/types/Miniature_Type_Inconnu_EV.png"

  shadowHintToggle:boolean = false;
  shadowHint:boolean = false;
  typeHint:boolean = false;
  genHint:number = 0;

  size:number = 100;
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
      this.getGame(this.size);
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
        this.url = url;
        setup.url = url.url;
        setup.size = size;
        this.pokemonService.getGame(setup);
      } 
    });
  }

  getHint(size:number) {
    let val = this.score - 15;
    if(this.verifScore(val)) {
      let setup:Setup = new Setup();
      this.image$.pipe(
        take(1)
      ).subscribe({
        next: (img) => {
          setup.url = this.url.url;
          setup.game = img.game;
          setup.size = size;
          this.pokemonService.getHint(setup);
        }
      })
    }
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

  toggleImage(event:any) {
    this.shadowHintToggle = !this.shadowHintToggle;
  }

  activeShadowHint() {
    let val = this.score - 50;
    if (this.verifScore(val)) {
      this.shadowHint = true;
      this.shadowHintToggle = !this.shadowHintToggle;
    }
  }

  revealType() {
    let val = this.score - 25;
    if (this.verifScore(val)) {
      this.typeHint = true;
    }
  }

  getGen() {
    let val = this.score - 20;
    if (this.verifScore(val)) {
      this.genHint = this.testGen(this.pokemon.dex);
    }
  }

  verifScore(val:number) : boolean {
    if (val < 0) {return false;}
    else {this.score = val; return true;}
  }

  anotherRound() {
    location.reload();
  }

  giveUp() {
    event?.preventDefault();
    this.nbTry=0;
    this.score=0;
    this.gameStatus = true;
  }

  testGen(dex:number) : number {
    let val:number = 0;
    switch(true) {
      case (dex <= 151):
        val = 1;
        break;
      case (dex <= 251):
        val = 2;
        break;
      case (dex <= 386):
        val = 3;
        break;  
      case (dex <= 493):
        val = 4;
        break;     
      case (dex <= 649):
        val = 5;
        break;  
      case (dex <= 721):
        val = 6;
        break;  
      case (dex <= 807):
        val = 7;
        break; 
      case (dex <= 898):
        val = 8;
        break; 
      default:
        val = 9;
        break;
    }
    return val;
  }
}
