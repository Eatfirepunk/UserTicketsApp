import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { TokenResponse } from '../models/UserModels/user';

@Injectable()
export class AuthService {

  private apiUrl = 'https://localhost:44353/api';
  private tokenKey = 'jwtToken';
  constructor(private http: HttpClient) { }

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
  }

  getToken(): string | null {
    // Get the token from local storage
    const token = localStorage.getItem(this.tokenKey);
    return token !== null ? token : null;
  }

  isLoggedIn(): boolean {
    // Check if the user is logged in (i.e. if the token exists in local storage or session storage)
    const token = this.getToken();
    return token !== null && token !== undefined;
  }

}

