import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import * as Leaflet from 'leaflet';
import { Observable, tap, map, delay, switchMap, take, catchError } from 'rxjs';
import { MapService } from '../../map.service';
import { MarkerService } from '../../marker.service';
import { Location } from '../../models/Location';
import { OSM } from '../../models/OSM';

const iconRetinaUrl = 'assets/marker-icon-2x.png';
const iconUrl = 'assets/marker-icon.png';
const shadowUrl = 'assets/marker-shadow.png';
const iconDefault = Leaflet.icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  tooltipAnchor: [16, -28],
  shadowSize: [41, 41]
});
Leaflet.Marker.prototype.options.icon = iconDefault;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements AfterViewInit {

  map!: Leaflet.Map;
  locations!: Location[];
  locations$!: Observable<Location[]>;
  selected$!: Observable<Location>;
  queries$!: Observable<any>;
  searchForm!: FormControl;

  constructor(
    private markerService: MarkerService,
     private mapService:MapService, 
      private formBuilder:FormBuilder) 
    {}

  ngOnInit() : void {
    this.initMap();
    this.initForm();
    this.initObservables();
    this.getLocations();
  }  

  ngAfterViewInit(): void {
    this.loadLocations();
  }

  initMap(): void {
    this.map = Leaflet.map('map', {
      center: [48.5734053,7.7521113],
      zoom: 6,
    });
    const tiles =  new Leaflet.TileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    } as Leaflet.TileLayerOptions);
    tiles.addTo(this.map);
  }

  initObservables() : void {
    this.locations$ = this.mapService.locations$;
    this.selected$ = this.markerService.selected$;
    this.queries$ = this.mapService.queries$;
  }

  initForm() : void {
    this.searchForm = this.formBuilder.control('');
  }

  setZoomOut() : void {
    this.map.setZoom(3);
  }

  getLocations(): void {
    this.mapService.getLocationsFromAPI();
  }

  loadLocations() : void {  
    this.locations$.pipe(
      tap((data) => {
        this.markerService.addMarkers(this.map,data); 
      })
    ).subscribe();
  }

  findLocation(query:string) : void {
    this.mapService.findLocationFromOSM(query);
  }

  setPinOnMap(osm:OSM) {
    let location:Location = new Location();
    location.latitude = parseFloat(osm.lat);
    location.longitude = parseFloat(osm.lon);
    location.name = osm.display_name
    this.markerService.addNewMarker(this.map,location);
    this.map.flyTo([location.latitude, location.longitude], this.map.getZoom());
  }
 
}