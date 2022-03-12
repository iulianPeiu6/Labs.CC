import { CommonModule } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { DxButtonModule, DxDataGridModule, DxFormModule, DxLoadIndicatorModule, DxLoadPanelModule } from 'devextreme-angular';
import ArrayStore from 'devextreme/data/array_store';
import 'devextreme/data/odata/store';
import notify from 'devextreme/ui/notify';
import { Movie, MovieService } from 'src/app/shared/services/movie.service';

@Component({
  templateUrl: 'movies.component.html'
})

export class MoviesComponent {
  dataSource: any;
  movies: Movie[];
  loadingVisible = false;

  constructor(private movieService: MovieService) {
    this.loadingVisible = true;
    this.dataSource =  new ArrayStore({
      key: ["id"],
      data: new Array<Movie>()
    });
    this.movies = new Array<Movie>();
  }

  async ngOnInit() {
    this.loadingVisible = true;
    this.movies = await this.movieService.getMovieRecommendations();

    this.dataSource =  new ArrayStore({
      key: ["id"],
      data: this.movies
    });
    this.loadingVisible = false;
  }
  async sendMoviesToMail(event: Event) {
    var response = await this.movieService.sendMoviesToEmail();
    if (response == true) {
      notify("Movies recommendations sent on email!", "success");
    } else {
      notify("Faile to send movies recommendations sent on email!", "error");
    }
  }
}

@NgModule({
  imports: [
    CommonModule,
    DxLoadIndicatorModule,
    DxLoadPanelModule, 
    DxDataGridModule,
    RouterModule,
    DxButtonModule,
    DxLoadIndicatorModule],
  exports: [MoviesComponent],
  declarations: [MoviesComponent],
  providers: [],
})
export class MoviesModule { }
