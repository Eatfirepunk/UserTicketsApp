import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth-service.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'TicketApp';
  isLoggedIn:boolean=false;
  constructor(
    private router: Router,
    private userService: UserService,
    private authService: AuthService,
    private cd: ChangeDetectorRef
  ) { 
    this.isLoggedIn = this.authService.isLoggedIn();

  }
  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    if(!this.isLoggedIn)
    {
      this.router.navigate(['/login']);
    }
  }



}
