import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, delay, switchMap, take, catchError } from 'rxjs';
import { environment } from 'src/environments/environments';
import { CVItem } from '../models/CVItem';

@Injectable({
  providedIn: 'root'
})

export class CvService {

  constructor(private http: HttpClient) { }

  private _cvItems$ = new BehaviorSubject<CVItem[]>([]);

  get cvItems$() {
    return this._cvItems$.asObservable();
  }

  getCVItemFromAPI() {
    this.http.get<CVItem[]>(`${environment.apiUrl}/cv/`).pipe(
      take(1),
      tap(CVs => {
        this._cvItems$.next(CVs);
      })
    ).subscribe();
  }
}
