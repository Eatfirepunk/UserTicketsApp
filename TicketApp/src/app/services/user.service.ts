import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import jwtDecode from 'jwt-decode';
import { User } from '../models/UserModels/user';

@Injectable()
export class UserService {

  private baseUrl = 'https://localhost:44353/api/Users/';
  private tokenKey = 'jwtToken';

  constructor(private http: HttpClient) { }



  private setHttpHeaders():HttpHeaders
  {
    const token = localStorage.getItem(this.tokenKey);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return headers;
  }
  public getAllUsers(): Observable<any[]> {
    const headers = this.setHttpHeaders();
    return this.http.get<any[]>(this.baseUrl, { headers });
  }

  public getUserById(id: number): Observable<any> {
    return this.http.get<any>(this.baseUrl + id);
  }

  public getUserByEmail(email: string | null): Observable<User> {
    const headers = this.setHttpHeaders();
    return this.http.get<User>(this.baseUrl+'email/' + email, { headers });
  }

  public createUser(user: any): Observable<any> {
    return this.http.post<any>(this.baseUrl, user);
  }

  public getUserCatalog(): Observable<any[]> {
    const headers = this.setHttpHeaders();
    return this.http.get<any[]>(`${this.baseUrl}catalog`, { headers });
  }

  public updateUser(id: number, user: any): Observable<any> {
    return this.http.put<any>(this.baseUrl + id, user);
  }

  public deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(this.baseUrl + id);
  }

  


}