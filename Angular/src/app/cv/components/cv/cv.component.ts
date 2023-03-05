import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CVItem } from '../../models/CVItem';
import { CvService } from '../../services/cv.service';

@Component({
  selector: 'app-cv',
  templateUrl: './cv.component.html',
  styleUrls: ['./cv.component.scss']
})
export class CvComponent implements OnInit {

  cvItems$!:Observable<CVItem[]>;

  url:string = "https://www.pokepedia.fr/Fichier:Deusolourdo_(Forme_Double)-EV.png";

  constructor(private cvService: CvService) {

  }

  firstCompo:CVItem = new CVItem();
  secondCompo: CVItem = new CVItem();

  ngOnInit(): void {
    this.initObservables();
    this.getItems();
    this.firstCompo.title = "Résumé";
    this.secondCompo.title = "Formations";
  }

  initObservables(): void {
    this.cvItems$ = this.cvService.cvItems$;
  }

  getItems() : void {
    this.cvService.getCVItemFromAPI();
  }

  
 
}
