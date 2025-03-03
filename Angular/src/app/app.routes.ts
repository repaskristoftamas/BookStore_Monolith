import { Routes } from '@angular/router';
import { AuthorComponent } from './author/author.component';
import { BookComponent } from './book/book.component';
import { GenreComponent } from './genre/genre.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    {path: '', component: HomeComponent, pathMatch: 'full', title: "Home - BookStore" },
    { path: 'authors', component: AuthorComponent, title: "Authors"},
    { path: 'books', component: BookComponent, title: "Books" },
    { path: 'genres', component: GenreComponent, title: "Genres" },
    { path: '**', redirectTo: '' }
];