import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {UsersComponent} from './components/users/users.component'
import {LoginComponent} from './components/login/login.component'
import { AuthService } from './services/auth-service.service';
import { UserService } from './services/user.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpHandler } from '@angular/common/http';
import { TopNavBarComponent } from './components/shared/top-nav-bar/top-nav-bar.component';
import { AboutComponent } from './components/about/about.component';
import { CreateOrEditUserComponent } from './components/users/create-or-edit-user/create-or-edit-user.component';
import { DialogModule } from 'primeng/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MultiSelectModule } from 'primeng/multiselect';

@NgModule({
  declarations: [
    AppComponent,
    UsersComponent,
    LoginComponent,
    TopNavBarComponent,
    AboutComponent,
    CreateOrEditUserComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    DialogModule,
    BrowserAnimationsModule,
    MultiSelectModule
  ],
  providers: [AuthService,UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
