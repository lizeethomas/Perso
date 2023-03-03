import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, delay, switchMap, take, catchError } from 'rxjs';
import { environment } from 'src/environments/environments';
import { map } from 'rxjs';
import { Movie } from '../models/movie.model';
import { TMDB } from '../models/tmdb.model';
import { Result } from '../models/result.model';

@Injectable()

export class MoviesService {

  constructor(private http: HttpClient) {}

  private _loading$ = new BehaviorSubject<boolean>(false);
  private _data$ = new BehaviorSubject<TMDB[]>([]);
  private _tmdb$ = new BehaviorSubject<TMDB>(new TMDB);
  private _movies$ = new BehaviorSubject<Movie[]>([]);

  get loading$() {
    return this._loading$.asObservable();
  }

  get data$() {
    return this._data$.asObservable();
  }

  get movies$() {
    return this._movies$.asObservable();
  }

  get tmdb$() {
    return this._tmdb$.asObservable();
  }

  private setLoadingStatus(loading: boolean) {
    this._loading$.next(loading);
  }

  getMoviesFromAPI() {
    this.setLoadingStatus(true);
    this.http.get<Movie[]>(`${environment.apiUrl}/movies/`).pipe(
      delay(0),
      tap(movies => {
        movies.forEach(m => m.editmode = false);
        this._movies$.next(movies);
        this.setLoadingStatus(false);
      })
    ).subscribe();
  }

  addNewMovie(movie:Movie) : Observable<any> {
    const headers:HttpHeaders = new HttpHeaders({
      "Content-Type" : "application/json"
    }); 
    return this.http.post<Movie>(`${environment.apiUrl}/movies/`, movie, {headers}).pipe();
  } 

  editMovie(movie:Movie) : Observable<any> {
    const headers:HttpHeaders = new HttpHeaders({
      "Content-Type" : "application/json"
    }); 
    return this.http.put<Movie>(`${environment.apiUrl}/movies/`, movie, {headers}).pipe();
  } 

  deleteMovie(movie:Movie) : Observable<any> {
    return this.http.delete(`${environment.apiUrl}/movies/${movie.id}`).pipe();
  }


  getDataFromTMDB(query:string) {
    this.setLoadingStatus(true);
    this.http.get<Result>(`${environment.movieUrl}`+query+"&append_to_response=credits").pipe(
      delay(0),
      tap((data:Result) => {
        this._tmdb$.next(data.results[0]);
        this._data$.next(data.results.sort((a, b) => this.getYearInNumber(a.release_date) - this.getYearInNumber(b.release_date)));
        this.setLoadingStatus(false);
      })
    ).subscribe((data) => console.log(data.results[0]));
  }

  convertTMDBintoMovie(tmdb:TMDB) : Movie {
    try {
      let movie = new Movie()
      movie.title = tmdb.original_title;
      movie.url = `${environment.posterUrl}` + tmdb.poster_path;
      movie.commentary = "Add new comment";
      movie.edited = new Date;
      movie.date = this.getYearInNumber(tmdb.release_date);
      movie.editmode = false;
      return movie;
    }
    catch {
      return new Movie;
    }

  }

  getYearInNumber(date:string) : number {
    return Number(date.split("-")[0]);
  }

}
