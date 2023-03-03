import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import *  as Leaflet from 'leaflet';
import { BehaviorSubject, Observable, tap, delay, switchMap, take, catchError } from 'rxjs';
import { Location } from './models/Location';
import { environment } from 'src/environments/environments';
import { Address, OSM } from './models/OSM';
import { Picture } from './models/Picture';

@Injectable({
  providedIn: 'root'
})

export class MapService {

    constructor(private http:HttpClient) {
    }

    private _locations$ = new BehaviorSubject<Location[]>([]);
    private _queries$ = new BehaviorSubject<OSM[]>([]);

    get locations$() {
      return this._locations$.asObservable();
    }

    get queries$() {
      return this._queries$.asObservable();
    }

    getLocationsFromAPI() {
      this.http.get<Location[]>(`${environment.apiUrl}/map/`).pipe(
        tap(locations => {
          this._locations$.next(locations);
        })
      ).subscribe();
    }

    findLocationFromOSM(query:string) {
      let url:string = 
      "https://nominatim.openstreetmap.org/search?q="
      + query
      + "&format=json&polygon=1&addressdetails=1";
      
      this.http.get<OSM[]>(url).pipe(
        take(1),
        tap((data) => {
          this._queries$.next(data);
        })
      ).subscribe()
    }

    addNewLocation(location: Location) : Observable<any> {
      const headers: HttpHeaders = new HttpHeaders({
        "Content-Type" : "application/json"
      });
      return this.http.post<Location>(`${environment.apiUrl}/map/`, location, {headers}).pipe();
    }

    deleteLocation(location:Location) : Observable<any> {
      return this.http.delete(`${environment.apiUrl}/map/${location.id}`).pipe();
    }

    addPictureToLocation(picture: Picture) : Observable<any> {
      const headers: HttpHeaders = new HttpHeaders({
        "Content-Type" : "application/json"
      });
      return this.http.put<Picture>(`${environment.apiUrl}/map/picture/`, picture, {headers}).pipe();
    }

}