import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of, tap } from 'rxjs';
import { TokenResponse, User } from '../models/UserModels/user';
import { UserService } from './user.service';
import jwtDecode from 'jwt-decode';
@Injectable()
export class AuthService {

  private apiUrl = 'https://localhost:44353/api';
  private tokenKey = 'jwtToken';
  private emailKey = 'emailUser';
  user:User | null = null;
  email:string | null = null;
  constructor(private http: HttpClient,private _userService:UserService) { }

  login(email: string, password: string): Observable<TokenResponse> {
    const body = { email, password };
    return this.http.post(`${this.apiUrl}/users/login`, body).pipe(
      tap((response: any) => {
        localStorage.setItem(this.tokenKey, response.token);
      }),
      map((response: any) => {
        return { token: response.token };
      })
    );
  }

  logout() {
    // Remove the token from local storage or session storage
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.emailKey);
  }

  getToken(): string | null {
    // Get the token from local storage
    const token = localStorage.getItem(this.tokenKey);
    return token !== null ? token : null;
  }

  getCurrentUser(): Observable<User | null> {
    if (!this.isLoggedIn) {
      return of(null);
    }
    if (localStorage.getItem(this.emailKey)) {
      return this._userService.getUserByEmail(localStorage.getItem(this.emailKey)).pipe(
        tap(result => this.user = result),
        map(result => this.user)
      );
    }
    return of(this.user);
  }

  public isLoggedIn(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    if(token)
    {
      var expiredToken = this.isJwtTokenExpired(token);
      if(expiredToken)
      {
        localStorage.removeItem(this.tokenKey);
        return false;
      }
    }
    else{return false;}
    
    return true;
  }

  private isJwtTokenExpired(token: string): boolean {
    const decodedToken: any = jwtDecode(token);
    console.log(decodedToken);
    const currentTime: number = Date.now() / 1000;
    let isExpired = decodedToken.exp < currentTime;
    if(isExpired)
    {
      localStorage.removeItem(this.emailKey);
    }
    else
    {
      localStorage.setItem(this.emailKey,decodedToken.email);
    }
    return isExpired;
  }

}


