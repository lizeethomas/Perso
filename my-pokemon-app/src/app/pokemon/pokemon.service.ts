import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, skip } from 'rxjs';
import { Pokemon } from './models/pokemon';
import { environments } from 'src/environments/environments';
import { Url } from './models/url';
import { Bitmap } from './models/bitmap';

@Injectable({
  providedIn: 'root'
})
export class PokemonService {

  constructor(private http:HttpClient) { }

  private _pokemon$ = new BehaviorSubject<Pokemon>(new Pokemon);
  private _url$ = new BehaviorSubject<Url>(new Url);
  private _image$ = new BehaviorSubject<Bitmap>(new Bitmap(0, 0));

  public imageSrc!: string;

  get pokemon$() {
    return this._pokemon$.asObservable();
  } 

  get url$() {
    return this._url$.asObservable();
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

  getBitmap(name:string) : void {
    if (name !== undefined) {
      this.http.get<Bitmap>(`${environments.apiUrl}/bitmap/`+ name).pipe(
        tap(b => {
            this._image$.next(b);
            console.log(b);
        })
      ).subscribe(); 
    }
  } 

  cropImg(url:string, size:number) : string {
    if (url !== undefined) {
      this.http.get(`${environments.apiUrl}/game/`+ url + '/' + size, {responseType:'blob'}).pipe(
      ).subscribe((response) => {
        const reader = new FileReader();
        reader.onloadend = () => {
          this.imageSrc = reader.result as string;
        };
        reader.readAsDataURL(response);
        return this.imageSrc;
      });
    }
    return "";
  }
}
