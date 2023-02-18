import { Component, OnInit } from '@angular/core';
import { User } from '../../models/UserModels/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  editingUser:User= {id: 0, username: '', email: '', roles: [],reportsToId:'',reportsToUsername:""};
  showDialog: boolean = false;
  action:string="Add";
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }

  saveUser(user: User = {id: 0, username: '', email: '', roles: [],reportsToId:"",reportsToUsername:""}): void {
    if(user.id)
    {
      this.editingUser = user;
      this.action = "Edit";
    }
    else
    {
      this.action="Add";
    }
    this.showDialog=true;
  }

  cancelEdit(){}
  deleteUser(user: User): void {
    // delete the user using the user service
  }
}