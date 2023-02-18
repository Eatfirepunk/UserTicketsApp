import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'TicketApp';
  constructor(
    private router: Router,
    private userService: UserService
  ) { 


  }
  ngOnInit(): void {
    console.log(this.userService.isLoggedIn());
    if(!this.userService.isLoggedIn())
    {
      this.router.navigate(['/login']);
    }
 
  }



}
