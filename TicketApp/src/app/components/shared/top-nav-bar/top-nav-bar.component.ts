import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth-service.service';

@Component({
  selector: 'app-top-nav-bar',
  templateUrl: './top-nav-bar.component.html',
  styleUrls: ['./top-nav-bar.component.css']
})
export class TopNavBarComponent implements OnInit {
  isLoggedIn = false;
  @Input() set loggedIn(loggedIn: boolean) {
    this.isLoggedIn = loggedIn;
  }

  constructor(private authService: AuthService,private router:Router) { }

  ngOnInit(): void {
 
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
