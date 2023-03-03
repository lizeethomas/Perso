import { Component, Input, OnInit, Output } from '@angular/core';
import { MovieGenres } from '../../enums/MovieGenres';
import { Movie } from '../../models/movie.model';
import { MoviesService } from '../../services/movies.service';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss']
})
export class MovieComponent implements OnInit {
 
  @Input() movie!: Movie;

  admin:boolean = false;

  values = Object.keys(MovieGenres);
  tags = Object.values(MovieGenres);

  genres:any[] = [];

  deleteMode:boolean = false;
  comDisplay:boolean = false;

  constructor(private moviesService:MoviesService) {

  }

  ngOnInit(): void {
    this.deleteMode = false;
    this.verifAdmin();
    this.values = this.values.slice(0,this.values.length);
    this.movie.genres.forEach(g => {
      for (let i = 0; i < this.values.length; i++) {
        if (g.toString() === this.values[i] ) {
          //console.log(this.tags[i],g)
          this.genres.push(<string>this.tags[i]);
        }
      }
    });
  }

  setEditMode() {
    this.movie.editmode = true;
  }

  setDeleteMode() {
    this.deleteMode = !this.deleteMode;
  }

  deleteMovie() {
    this.moviesService.deleteMovie(this.movie).subscribe();
    location.reload();
  }

  changeComDisplay() {
    this.comDisplay = !this.comDisplay;
  }


  verifAdmin() {
    let token:any = localStorage.getItem("token");
    if (token != null) {
      this.admin = true;
    }
  }
 
}
