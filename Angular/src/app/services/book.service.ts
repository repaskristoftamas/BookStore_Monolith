import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Book } from '../models/book';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = `${environment.apiUrl}/books`;

  constructor(private httpClient: HttpClient) { }

    getBooks(params: { [key: string]: any } = {}) : Observable<Book[]> {
      let httpParams = new HttpParams();

      Object.keys(params).forEach((key) => {
        if (params[key] !== undefined && params[key] !== null) {
          httpParams = httpParams.set(key, params[key]);
        }
      });

      return this.httpClient.get<Book[]>(this.apiUrl, { params: httpParams });
    }

    getBookById(bookId: number): Observable<Book> {
      return this.httpClient.get<Book>(`${this.apiUrl}/${bookId}`).pipe(
        map(book => ({
          ...book,
          author: book.author
        }))
      );
    }

    addNewBook(bookData: any) : Observable<Book> {
      return this.httpClient.post<Book>(this.apiUrl, bookData);
    }
}
