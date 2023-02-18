import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DropdownDto, SelectItem } from '../../../models/selectItem';
import { Role, User } from '../../../models/UserModels/user';
import { UserService } from '../../../services/user.service';


@Component({
  selector: 'app-create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html'
})
export class CreateOrEditUserComponent implements OnInit {
  userForm: FormGroup;
  isAddMode: boolean = false;
  selectedRoles: SelectItem[] = [];
  allUsers: DropdownDto[] = [];


  @Input() set user(user: User) {
    this.isAddMode = !user;
    this.updateForm(user);
  }
  ngOnInit(): void {
    this.userService.getUserCatalog().subscribe((result)=> {this.allUsers = result;});
  }

  @Output() onSave = new EventEmitter<User>();
  @Output() onCancel = new EventEmitter<void>();

  constructor(private fb: FormBuilder,private userService: UserService) {
    this.userForm = this.fb.group({
      id: [],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      reportsToId: []
    });
  }

  onSubmit() {
    const user = this.userForm.value;
    this.onSave.emit(user);
    console.log(user);
  }

  onCancelClick() {
    this.onCancel.emit();
  }

  private updateForm(user: User) {
    if (user.id) {
      this.userForm.patchValue(user);
    } else {
      this.userForm.reset();
    }
  }

}
