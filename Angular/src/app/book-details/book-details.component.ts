import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Book } from '../models/book';

@Component({
  selector: 'app-book-details',
  imports: [CommonModule, FormsModule],
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css'
})
export class BookDetailsComponent {
  @Input() book!: Book;
  @Output() closeModal = new EventEmitter<void>();
  @Output() saveBook = new EventEmitter<Book>();

  close() {
    this.closeModal.emit();
  }

  save() {
    this.saveBook.emit(this.book);
    this.close();
  }
}
