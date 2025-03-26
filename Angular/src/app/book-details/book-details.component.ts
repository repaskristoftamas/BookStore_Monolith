import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Book } from '../models/book';
import { BookService } from '../services/book.service';

@Component({
  selector: 'app-book-details',
  imports: [CommonModule, FormsModule],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent {
  @Input() book!: Book;
  @Input() authorName: string = ''
  @Output() authorNameChange = new EventEmitter<string>();
  @Output() closeModal = new EventEmitter<void>();
  @Output() saveBook = new EventEmitter<Book>();

  constructor(private bookService: BookService) {}

  close() {
    this.closeModal.emit();
  }

  save() {
    console.log(this.book.author);
    if (!this.book || !this.book.title || !this.book.author) {
      console.log(this.book.author);
      console.error("Book is not defined or missing title or author!");
      return;
    }

    const bookData = {
      title: this.book.title,
      description: this.book.description,
      author: {
        id: this.book.author.id || 0,
        name: this.authorName || ''
      },
      genre: {
        id: this.book.genre.id || 0,
        name: this.book.genre.name ||''
      }
    }

    console.log(bookData);

    this.bookService.addNewBook(bookData).subscribe({
      next: (response) => {
        console.log("Book saved successfully:", response);
        this.saveBook.emit(response);
        this.close();
        //fetch books but only refresh with the new book
      },
      error: (error) => console.error("Error saving new book:", error)
    });
  }

  updateAuthorName(value: string) {
    this.authorName = value;
    this.authorNameChange.emit(this.authorName);
  }
}
