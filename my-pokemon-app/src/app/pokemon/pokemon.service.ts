import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, skip } from 'rxjs';
import { Pokemon } from './models/pokemon';
import { environments } from 'src/environments/environments';
import { Url } from './models/url';

@Injectable({
  providedIn: 'root'
})
export class PokemonService {

  constructor(private http:HttpClient) { }

  private _pokemon$ = new BehaviorSubject<Pokemon>(new Pokemon);
  private _url$ = new BehaviorSubject<Url>(new Url);

  get pokemon$() {
    return this._pokemon$.asObservable();
  } 

  get url$() {
    return this._url$.asObservable();
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
}
