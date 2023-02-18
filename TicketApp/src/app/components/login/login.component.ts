import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LogModel } from '../../models/UserModels/user';
import { AuthService } from '../../services/auth-service.service';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  error: string = '';
  logModel!: LogModel;
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { 


  }

  ngOnInit(): void {
    
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.logModel = new LogModel();
  }

   onSubmit() {
     this.authService.login(this.logModel.email, this.logModel.password).subscribe(
     success => {
        this.router.navigate(['/']);
      },
      error => {
        this.error = 'Invalid username or password';
       }
    );
  }

}
