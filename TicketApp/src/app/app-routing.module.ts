import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import {LoginComponent} from './components/login/login.component'
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [{ path: 'login', component: LoginComponent },{ path: 'users', component: UsersComponent },{ path: 'about', component: AboutComponent }];


@NgModule({
  imports: [BrowserModule,RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
