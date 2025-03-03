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

  isModalOpen = false;
  selectedBook: Book | null = null;
  authorName: string = '';
  isDisabled: boolean = true;

  constructor(private bookService: BookService, private authorService: AuthorService) {}

  ngOnInit() {
    this.fetchBooks();
  }

  fetchBooks() : void {
    this.books = [];

    const queryParams: any = {
      Query: this.query || undefined,
      FilterBy: this.filterBy || undefined,
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
    this.fetchBooks();
  }

  openModal(book: Book, event: Event) {
    event.stopPropagation();
    this.selectBookLazyLoading(book);
    this.selectedBook = { ...book };
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedBook = null;
  }

  selectBookLazyLoading(book: Book) {
    if(book.id && !book.author?.id) {
      this.bookService.getBookById(book.id).subscribe(fetchedBook => {
        if(this.selectedBook && fetchedBook.author) {
          this.authorName = fetchedBook.author.name;
        }
      })
    }
  }

  addNewBook(event: Event) {
    event.stopPropagation();
    this.selectedBook = {
      title: '',
      description: '',
      author: { id: 0, name: '' },
      genre: { id: 0, name: '' }
    };
    this.authorName = '';
    this.isModalOpen = true;
  }

  updateBook(updatedBook: Book) {
    const index = this.books.findIndex(b => b.id === updatedBook.id);
    if (index !== -1) {
      this.books[index] = updatedBook;
    }
  }
}