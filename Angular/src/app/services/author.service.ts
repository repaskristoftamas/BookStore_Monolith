import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Author } from '../models/author';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = `${environment.apiUrl}/authors`;

  constructor(private httpClient: HttpClient) { }

  getAuthors(name?: string): Observable<Author[]> {
    const url = name ? `${this.apiUrl}?query=${encodeURIComponent(name)}` : this.apiUrl;
    return this.httpClient.get<Author[]>(url);
  }
  
}
