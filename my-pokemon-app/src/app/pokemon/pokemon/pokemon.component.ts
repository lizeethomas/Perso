import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Pokemon } from '../models/pokemon';
import { PokemonService } from '../pokemon.service';
import { Observable, tap, delay, take, switchMap } from 'rxjs';
import { Url } from '../models/url';
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
  game$!:Observable<string[]>;

  pokemon!:Pokemon;
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
  cmpt:number = 0;

  averageScore:number = 0;

  gameStatus!:boolean;
  seeScore!:boolean;

  constructor(private pokemonService: PokemonService, private fb:FormBuilder) { }

   ngOnInit(): void {
    this.gameStatus = false;
    this.score = 200;
    this.initFormControl();
    this.initObservable();
    this.getPokemon();
   }

   ngAfterViewInit(): void { 
    this.getGame(this.size);
   }

  initObservable() : void  {
    this.pokemon$ = this.pokemonService.pokemon$;
    this.game$ = this.pokemonService.game$;
  }

  initFormControl() : void  {
    this.guessCtrl = this.fb.control("", Validators.required);
  }

  getPokemon() : void  {
    let dex:number = this.pokemonService.genRandomDex();
    this.pokemonService.getPokemon(dex);
  }

  getGame(size:number) : void {
    let setup:Setup = new Setup();
    this.pokemon$.pipe().subscribe({
      next: (pkmn:Pokemon) => {
        this.pokemon = pkmn;
        this.type1 = "../../assets/types/Miniature_Type_" + pkmn.type1 + "_EV.png";
        if (pkmn.type2 !== undefined && pkmn.type2 !== null) {
          this.type2 = "../../assets/types/Miniature_Type_" + pkmn.type2 + "_EV.png";
        }
        setup.url = pkmn.url;
        setup.size = size;
        this.pokemonService.getGame(setup);
      }
    })
  }

  onSubmit() : void {
    this.nbTry++;
    event?.preventDefault();
    this.try = this.guessCtrl.value;
    if (this.testWin(this.try, this.pokemon.name)) {
      this.gameStatus = true;
      console.log(this.pokemon.name)
      this.editScore();
    }
    else {
      this.initFormControl();
      let val = this.score - 5;
      this.verifScore(val);
    }
  } 

  testWin(str1:string, str2:string) : boolean {
    const v1 = str1.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    const v2 = str2.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    return v1.localeCompare(v2, "en", { sensitivity: "base" }) === 0;
  }

  newReveal() : void {
    let val = this.score - 15;
    if (this.verifScore(val) && this.cmpt < 5) {
      this.cmpt++;
    }
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
    this.editScore();
  }

  editScore() {
    let found = localStorage.getItem("found");
    let score = localStorage.getItem("score");
    if (found !== null && score !== null) {
      let tmp = parseInt(found)+1;
      localStorage.setItem("found", tmp.toString());
      tmp = parseInt(score)+this.score;
      localStorage.setItem("score", tmp.toString()); 
    } else {
      localStorage.setItem("found", "1");
      localStorage.setItem("found", this.score.toString());
    }
  }
  
  checkScore() {
    this.seeScore = !this.seeScore;
    let found = localStorage.getItem("found");
    let score = localStorage.getItem("score");
    if (found !== null && score !== null) {
      let num:number = parseFloat(score);
      let denum:number = parseFloat(found);
      this.averageScore = (num/denum);
    }
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
