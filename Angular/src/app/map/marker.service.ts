import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import *  as Leaflet from 'leaflet';
import { MapService } from './map.service';
import { Location } from './models/Location';
import { BehaviorSubject, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MarkerService {

  constructor(private http: HttpClient, private mapService:MapService) {

  }

  private _selected$ = new BehaviorSubject<Location>(new Location);

  get selected$() {
    return this._selected$.asObservable();
  }

  addMarkers(map: Leaflet.Map, locations: Location[]) : void {
    locations.forEach( l => {
      let marker = this.markerCustom(l);
      marker.addTo(map);
    });
  }

  addNewMarker(map: Leaflet.Map, location: Location) : void {
    let marker = this.markerCustom(location);
    marker.addTo(map);
  }

  markerCustom(location:Location) : Leaflet.Marker {
    let marker = Leaflet.marker([location.latitude, location.longitude]);
    //marker.bindPopup(location.name);
    marker.on('click', event => {
      this.openLocationInfo(location)
    });
    return marker;
  }

  openLocationInfo(location:Location) {
    this._selected$.next(location);
    this.selected$.pipe(
      take(1),
      //tap((data) => {console.log(data)})
    ).subscribe();
  }

}
