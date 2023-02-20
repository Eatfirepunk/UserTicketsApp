import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable, of, switchMap } from 'rxjs';
import { AuthService } from '../services/auth-service.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService,   private router: Router) {}
  
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> {
    return this.authService.getCurrentUser().pipe(
      switchMap(user => {
        console.log(user);
        if (user) {
          let isAdmin = user.roles.some(rol => rol.name.includes('Admin'));
          if (!isAdmin) {
            return of(this.router.parseUrl('/'));
          }
          return of(true);
        } else {
          // Redirect the user to the home page if they are not authenticated
          return of(this.router.parseUrl('/login'));
        }
      })
    );
  }
  
  
  
  
  

  
}
