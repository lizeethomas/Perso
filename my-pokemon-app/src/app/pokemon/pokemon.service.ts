import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, skip } from 'rxjs';
import { Pokemon } from './models/pokemon';
import { environments } from 'src/environments/environments';
import { Url } from './models/url';
import { Setup } from './models/setup';
import { Game } from './models/game';

@Injectable({
  providedIn: 'root'
})
export class PokemonService {

  constructor(private http:HttpClient) { }

  private _pokemon$ = new BehaviorSubject<Pokemon>(new Pokemon);
  private _url$ = new BehaviorSubject<Url>(new Url);
  private _shadow$ = new BehaviorSubject<Url>(new Url);
  private _image$ = new BehaviorSubject<Game>(new Game);

  public imageSrc!: string;

  get pokemon$() {
    return this._pokemon$.asObservable();
  } 

  get url$() {
    return this._url$.asObservable();
  }

  get shadow$() {
    return this._shadow$.asObservable();
  }


  get image$() {
    return this._image$.asObservable();
  }

  getRandomPokemon() : void {
    let nb:number = Math.round(Math.random() * (1010 - 1) + 1);
    this.http.get<Pokemon>(`${environments.apiUrl}/pokemon/` + nb).pipe(
      tap(p => {
        this._pokemon$.next(p);
      })
    ).subscribe();
  }

  getImgUrl(name:string) : void {
    if (name !== undefined) {
      this.http.get<Url>(`${environments.apiUrl}/image/`+ name).pipe(
        tap(u => {
            this._url$.next(u);
            console.log(name);
        })
      ).subscribe(); 
    }
  } 

  getShadow(name:string) : void {
    if (name !== undefined) {
      this.http.get<Url>(`${environments.apiUrl}/shadow/`+ name).pipe(
        tap(s => {
          this._shadow$.next(s);
        })
      ).subscribe();
    }
  }

  getGame(setup:Setup) : void {
    if (setup.url !== undefined) {
      //console.log(setup.url);
      const headers:HttpHeaders = new HttpHeaders({
        "Content-Type" : "application/json"
      });
      this.http.post<Game>(`${environments.apiUrl}/game/setup`, setup, {headers}).pipe(
        tap((game:Game) => {
          this._image$.next(game);
        })
      ).subscribe();
    }
  }

}
