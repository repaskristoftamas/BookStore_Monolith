import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GenreService } from '../services/genre.service';
import { Genre } from '../models/genre';

@Component({
  selector: 'app-genre',
  imports: [CommonModule, FormsModule],
  templateUrl: './genre.component.html',
  styleUrl: './genre.component.css'
})
export class GenreComponent {
  genres: Genre[] = [];
  query: string = '';

  constructor(private genreService: GenreService) {}

  ngOnInit() {
    this.fetchGenres();
  }

  fetchGenres() {
    this.genres = [];

    this.genreService.getGenres(this.query).subscribe({
      next: (data) => this.genres = data,
      error: (err) => {
        console.error('An error happened', err);
        this.genres = [];
      }
    });
  }
}
