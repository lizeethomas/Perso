import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MapService } from '../map.service';
import { Location } from '../models/Location';
import { Picture } from '../models/Picture';

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.scss']
})
export class LocationComponent implements OnInit {

  constructor(private mapService:MapService, private fb:FormBuilder) {

  }

  @Input() location!: Location;

  mainForm!:FormGroup;
  titleCtrl!:FormControl;
  urlCtrl!:FormControl;
  descriptionCtrl!:FormControl;

  picture:Picture = new Picture();
  editMode:boolean = false;

  admin:boolean = false;


  ngOnInit(): void {
    this.verifAdmin();
    this.initFormControls();
    this.initMainForm();
  }

  onSave(newLocation:Location) {
    console.log(newLocation);
    this.mapService.addNewLocation(newLocation).subscribe();
    location.reload();
  }

  private verifAdmin() {
    let token:any = localStorage.getItem("token");
    if (token != null) {
      this.admin = true;
    }
  }

  private initFormControls() : void {
    this.titleCtrl = this.fb.control(this.picture.title, Validators.required);
    this.descriptionCtrl = this.fb.control(this.picture.description,Validators.required);
    this.urlCtrl = this.fb.control(this.picture.pictureUrl,Validators.required);
  }

  private initMainForm() :  void {
    this.mainForm = this.fb.group({
      title: this.titleCtrl,
      url: this.urlCtrl,
      description: this.descriptionCtrl,
    });
  }

  setEditMode() : void {
    this.editMode = !this.editMode;
  }

  savePicture() {
    this.picture = this.mainForm.value;
    this.picture.id = this.location.id;
    this.mapService.addPictureToLocation(this.picture).subscribe();
    location.reload();
  }

  deleteLocation() :  void {
    this.mapService.deleteLocation(this.location).subscribe();
    location.reload();
  }

}
