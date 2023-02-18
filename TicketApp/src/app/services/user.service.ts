import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import jwtDecode from 'jwt-decode';

@Injectable()
export class UserService {

  private baseUrl = 'https://localhost:44353/api/Users';
  private tokenKey = 'jwtToken';

  constructor(private http: HttpClient) { }

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

  public getAllUsers(): Observable<any[]> {
    const token = localStorage.getItem(this.tokenKey);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any[]>(this.baseUrl, { headers });
  }

  public getUserById(id: number): Observable<any> {
    return this.http.get<any>(this.baseUrl + id);
  }

  public createUser(user: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, user);
  }

  public getUserCatalog(): Observable<any[]> {
    const token = localStorage.getItem(this.tokenKey);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<any[]>(`${this.baseUrl}/catalog`, { headers });
  }

  public updateUser(id: number, user: any): Observable<any> {
    return this.http.put<any>(this.baseUrl + id, user);
  }

  public deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(this.baseUrl + id);
  }

  
  private isJwtTokenExpired(token: string): boolean {
    const decodedToken: any = jwtDecode(token);
    console.log(decodedToken);
    const currentTime: number = Date.now() / 1000;
    return decodedToken.exp < currentTime;
  }

}