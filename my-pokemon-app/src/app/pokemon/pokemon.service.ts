import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, skip, map } from 'rxjs';
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
  private _game$ = new BehaviorSubject<Game>(new Game);

  get pokemon$() {
    return this._pokemon$.asObservable();
  } 

  get game$() {
    return this._game$.asObservable().pipe(
      map((game: Game) => {
        const images: string[] = [];
        images.push(game.start);
        images.push(game.hint1);
        images.push(game.hint2);
        images.push(game.hint3);
        images.push(game.hint4);
        images.push(game.hint5);
        images.push(game.shadow);
        return images;
      })
    )
  }

  genRandomDex() : number {
    let dex:number = Math.round(Math.random() * (1010 - 1) + 1);
    return dex;
  }

  getPokemon(dex:number) : void {
    this.http.get<Pokemon>(`${environments.apiUrl}/pokemon/` + dex).pipe(
      tap(p => {
        this._pokemon$.next(p);
      })
    ).subscribe();
  }

  getGame(setup:Setup) : void {
    if (setup.url !== undefined) {
      console.log(setup);
      const headers:HttpHeaders = new HttpHeaders({
        "Content-Type" : "application/json"
      });
      this.http.post<Game>(`${environments.apiUrl}/game/`, setup, {headers}).pipe(
        tap((game:Game) => {
          this._game$.next(game);
          console.log(game);
        })
      ).subscribe();
    } 
  }


}

//   getRandomPokemon() : void {
//     let nb:number = Math.round(Math.random() * (1010 - 1) + 1);
//     this.http.get<Pokemon>(`${environments.apiUrl}/pokemon/` + nb).pipe(
//       tap(p => {
//         this._pokemon$.next(p);
//       })
//     ).subscribe();
//   }

//   getImgUrl(name:string) : void {
//     if (name !== undefined) {
//       this.http.get<Url>(`${environments.apiUrl}/image/`+ name).pipe(
//         tap(u => {
//             this._url$.next(u);
//         })
//       ).subscribe(); 
//     }
//   } 

//   getShadow(name:string) : void {
//     if (name !== undefined) {
//       this.http.get<Url>(`${environments.apiUrl}/shadow/`+ name).pipe(
//         tap(s => {
//           this._shadow$.next(s);
//         })
//       ).subscribe();
//     }
//   }

//   getGame(setup:Setup) : void {
//     if (setup.url !== undefined) {
//       //console.log(setup.url);
//       const headers:HttpHeaders = new HttpHeaders({
//         "Content-Type" : "application/json"
//       });
//       this.http.post<Game>(`${environments.apiUrl}/game/setup`, setup, {headers}).pipe(
//         tap((game:Game) => {
//           this._image$.next(game);
//           //console.log(game.game);
//           //console.log(game.url);
//         })
//       ).subscribe();
//     }
//   }

//   getHint(setup:Setup) : void {
//     if (setup.url !== undefined && setup.game !== undefined) {
//       //console.log(setup.url);
//       const headers:HttpHeaders = new HttpHeaders({
//         "Content-Type" : "application/json"
//       });
//       this.http.post<Game>(`${environments.apiUrl}/game/hint`, setup, {headers}).pipe(
//         tap((game:Game) => {
//           this._image$.next(game);
//           //console.log(game.url);
//         })
//       ).subscribe();
//     }
//   }

// }
