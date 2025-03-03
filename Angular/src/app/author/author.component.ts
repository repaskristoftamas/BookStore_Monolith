import { Component, Query } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthorService } from '../services/author.service';
import { Author } from '../models/author';

@Component({
  selector: 'app-author',
  imports: [CommonModule, FormsModule],
  templateUrl: './author.component.html',
  styleUrl: './author.component.css'
})
export class AuthorComponent {
  authors: Author[] = [];
  query: string = '';

  constructor(private authorService: AuthorService) {}

  ngOnInit() {
    this.fetchAuthors();
  }

  fetchAuthors() {
    this.authors = [];

    this.authorService.getAuthors(this.query).subscribe({
      next: (data) => this.authors = data,
      error: (err) => {
        console.error('An error happened', err);
        this.authors = [];
      }
    });
  }
}
