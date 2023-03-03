import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { combineLatest, map, Observable, startWith, debounceTime, distinctUntilChanged, tap, switchMap, take } from 'rxjs';
import { MoviesService } from '../../services/movies.service';
import { Movie } from '../../models/movie.model';
import { TMDB } from '../../models/tmdb.model';
import { Check } from '../../enums/Check';
import { environment } from 'src/environments/environments';
import { MovieEditComponent } from '../movie-edit/movie-edit.component';
import { LoginService } from 'src/app/core/services/login.service';
import { Sorting } from '../../enums/Sorting';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})



export class MoviesComponent implements OnInit {

  searchForm!:FormControl;
  check!:Check;

  Check = Check;

  loading$!:Observable<boolean>;
  movies$!:Observable<Movie[]>;
  data$!:Observable<TMDB[]>;
  tmdb$!:Observable<TMDB>;
  movie!:Movie;

  admin:boolean = false;

  activeClass1:boolean = false;
  activeClass2:boolean = false;
  activeClass3:boolean = true;
  activeClass4:boolean = false;


  activeClass:number = 1;
  sortingClass:number[] = [0,0,1,0];


  posterURL:string = `${environment.posterUrl}`;

  constructor(private formBuilder: FormBuilder, private moviesService: MoviesService, private loginService:LoginService) {

  }

  verifAdmin() {
    let token:any = localStorage.getItem("token");
    if (token != null) {
      this.admin = true;
    }
  }

  ngOnInit(): void {
    this.check = Check.empty;
    this.verifAdmin();
    this.initForm();
    this.initObservables();
    this.getMovies();
    this.sortByEdit();
  }

  private initForm() {
    this.searchForm = this.formBuilder.control('');
  }

  private initObservables() {
    this.loading$ = this.moviesService.loading$;
    this.movies$ = this.moviesService.movies$;
    this.tmdb$ = this.moviesService.tmdb$;
    this.data$ = this.moviesService.data$;
  }

  public getMovies() {
    this.moviesService.getMoviesFromAPI();
  }

  public getData(query:string) {
    this.check = Check.pending;
    this.moviesService.getDataFromTMDB(query);
    //setTimeout(() => {this.convert(); this.check = Check.found;}, 1000);
    //setTimeout(() => {this.check = Check.found;}, 1000);
    
  }

  public convert() {
    this.tmdb$.pipe().subscribe({
      next: (data) => {this.movie = this.moviesService.convertTMDBintoMovie(data);},
    });
  }

  public moviePicker(tmdb:TMDB) {
    this.movie = this.moviesService.convertTMDBintoMovie(tmdb);
    console.log(this.movie);
    this.check = Check.found;
  }

  sortByTitle() {
    this.sortingClass=[this.activeClass,0,0,0];
    switch (this.activeClass) {
      case 0: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.title > b.title ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case 1: {
        this.activeClass = -1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.title < b.title ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case -1: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.title > b.title ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      default: {
        return null;
      }
    }
  }

  sortByRelease() {
    this.sortingClass=[0,this.activeClass,0,0];
    switch (this.activeClass) {
      case 0: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.date < b.date ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case 1: {
        this.activeClass = -1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.date > b.date ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case -1: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.date < b.date ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      default: {
        return null;
      }
    }
  }

  sortByEdit() {
    this.sortingClass=[0,0,this.activeClass,0];
    switch (this.activeClass) {
      case 0: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.edited < b.edited ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case 1: {
        this.activeClass = -1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.edited > b.edited ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case -1: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.edited < b.edited ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      default: {
        return null;
      }
    }
  }

  sortByRating() {
    this.sortingClass=[0,0,0,this.activeClass];
    switch (this.activeClass) {
      case 0: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.rating < b.rating ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case 1: {
        this.activeClass = -1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.rating > b.rating ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      case -1: {
        this.activeClass = 1;
        //console.log(this.activeClass);
        this.movies$ = this.movies$.pipe(
          map((data) => {
            data.sort((a,b) => {return a.rating < b.rating ? -1 : 1});
            return data;
          })
        );
        return null;
      }
      default: {
        return null;
      }
    }
  }
 
}
