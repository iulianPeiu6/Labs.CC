import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

export class Movie {
  constructor(
    public id: string,
    public title: string, 
    public releaseDate: Date, 
    public rating: number,
    public posterUrl: string,
    public popularity: number,
    public numberOfRatings: number,
    public overview: string,
    public youtubeTrailerLink: string) { 
  }
}

@Injectable()
export class MovieService {
  constructor(private http: HttpClient) { }

  async getMovieRecommendations(): Promise<Movie[]> {
    const response = await this.http
        .get<Movie[]>('/api/1/Movie/GetRecommendations')
        .toPromise();

    console.log(response);
    return response;
  }

  async sendMoviesToEmail(): Promise<boolean> {
    const response = await this.http
        .post<boolean>('/api/1/Notification/SendRecommendationsViaEmail', null, { params: { email: "iulianpeiu6@gmail.com"}})
        .toPromise();

    console.log(response);
    return response;
  }
}