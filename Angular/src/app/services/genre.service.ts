import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Genre } from '../models/genre';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  private apiUrl = `${environment.apiUrl}/genres`;

  constructor(private httpClient: HttpClient) { }

  getGenres(name?: string): Observable<Genre[]> {
    const url = name ? `${this.apiUrl}?query=${encodeURIComponent(name)}` : this.apiUrl;
    return this.httpClient.get<Genre[]>(url);
  }
}
