import { Component, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MovieGenres } from '../../enums/MovieGenres';
import { Movie } from '../../models/movie.model';
import { MoviesService } from '../../services/movies.service';

@Component({
  selector: 'app-movie-edit',
  templateUrl: './movie-edit.component.html',
  styleUrls: ['./movie-edit.component.scss']
})
export class MovieEditComponent implements OnInit {
 
  @Input() movie!: Movie;

  mainForm!:FormGroup;
  titleCtrl!: FormControl;
  directorCtrl!:FormControl;
  dateCtrl!:FormControl;
  ratingCtrl!:FormControl;
  commentaryCtrl!:FormControl;
  genresCtrl!:FormControl;

  rate:number[] = [0,1,2,3,4,5,6,7,8,9,10];
  values = Object.values(MovieGenres);
  values2 = Object.keys(MovieGenres);
  checks:boolean[] = new Array(this.values.length);
  
  genres:MovieGenres[] = [];

  poster!:string;

  constructor(private fb: FormBuilder, private moviesService: MoviesService, private router:Router) {}

  ngOnInit(): void {
    for(let i = 0; i < this.checks.length; i++) { this.checks[i] = false}
    this.initFormControls();
    this.initMainForm();
    this.poster = this.movie.url;
  }

  private initMainForm(): void {
    this.mainForm = this.fb.group({
      title: this.titleCtrl,
      director: this.directorCtrl, 
      date: this.dateCtrl, 
      rating: this.ratingCtrl,
      commentary: this.commentaryCtrl, 
      genres: this.genresCtrl,
    })
  }

  private initFormControls() : void {
    this.titleCtrl = this.fb.control(this.movie.title, Validators.required);
    this.directorCtrl = this.fb.control(this.movie.director, Validators.required);
    this.dateCtrl = this.fb.control(this.movie.date,Validators.required);
    this.ratingCtrl = this.fb.control(this.movie.rating,Validators.required);
    this.genresCtrl = this.fb.control(this.movie.genres,Validators.required);
    this.checkChecks();
    //console.log(this.checks);
    this.commentaryCtrl = this.fb.control(this.movie.commentary,Validators.required);
  }

  checkChecks() : void {
    if(this.movie.genres != null) {
      this.movie.genres.forEach(g => {
        for(let i = 0; i < this.values.length; i++){
          if (g.toString() === this.values2[i] ) {
            let gg:any = g;
            this.genres.push(<MovieGenres>gg);
            this.checks[i] = true;
          }
        }
      });
    }
  }

  public refresh() {
    this.titleCtrl.setValue(this.movie.title);
    this.directorCtrl.setValue(this.movie.director);
    this.dateCtrl.setValue(this.movie.date);
    this.commentaryCtrl.setValue("");
  }

  cancelEditMode() {
    this.movie.editmode = false;
  }

  resetForm() : void {
    this.mainForm.reset();
  }

  onSubmitForm(): void {
    this.movie = this.mainForm.value;
    this.movie.genres = this.genres;
    this.movie.url = this.poster;
    console.log(this.movie);
    this.moviesService.addNewMovie(this.movie).subscribe();
    location.reload();
  }

  onSaveChanges(): void {
    this.movie = this.mainForm.value;
    this.movie.genres = this.genres;
    this.movie.url = this.poster;
    console.log(this.movie);
    this.moviesService.editMovie(this.movie).subscribe();
    location.reload();
  } 

  onCheckboxChange(e:any) { 
    if (e.target.checked) {
      this.genres.push( +<MovieGenres>e.target.value )
    } else {
      let index:number = this.genres.indexOf(+<MovieGenres>e.target.value);
      if (index !== -1) {
        this.genres.splice(index,1);
      }
    }
  }

}
