import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookService } from '../services/book.service';
import { AuthorService } from '../services/author.service';
import { Book } from '../models/book';
import { BookDetailsComponent } from '../book-details/book-details.component';

@Component({
  selector: 'app-book',
  imports: [CommonModule, FormsModule, BookDetailsComponent],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class BookComponent {
  books: Book[] = [];
  query: string = '';
  filterBy: string = '';
  includeAuthor: boolean = false;
  includeGenre: boolean = false;

  isModalOpen = false;
  selectedBook: Book | null = null;

  constructor(private bookService: BookService, private authorService: AuthorService) {}

  ngOnInit() {
    this.fetchBooks();
  }

  fetchBooks() : void {
    this.books = [];

    const queryParams: any = {
      Query: this.query || undefined,
      FilterBy: this.filterBy || undefined,
      IncludeAuthor: this.includeAuthor,
      IncludeGenre: this.includeGenre
    };

    this.bookService.getBooks(queryParams).subscribe({
      next: (data) => this.books = data,
      error: (err) => {
        console.error('An error happened', err);
        this.books = [];
      }
    });
  }

  getPlaceholder(): string {
    switch (this.filterBy) {
      case 'title': return 'Search by Title (exact match)';
      case 'author': return 'Search by Author (exact match)';
      case 'genre': return 'Search by Genre (exact match)';
      default: return 'No filter selected';
    }
  }

  clearQueryIfNoFilter(): void {
    if (!this.filterBy) {
      this.query = '';
    }
  }

  selectBookLazyLoading(book: Book){
    if(!book.author) {
      this.authorService.getAuthorByBookId(book.id).subscribe(author => {
        if(this.selectedBook) {
          this.selectedBook.author = author;
        }
      });
    }
  }

  openModal(book: Book) {
    this.selectBookLazyLoading(book);
    this.selectedBook = { ...book };
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedBook = null;
  }

  updateBook(updatedBook: Book) {
    const index = this.books.findIndex(b => b.id === updatedBook.id);
    if (index !== -1) {
      this.books[index] = updatedBook;
    }
  }
}