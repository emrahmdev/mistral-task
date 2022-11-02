import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';
import { InputErrorStateMatcher } from 'src/app/core/utils/input-error-state-matcher';
import { Router } from "@angular/router";

@Component({
    selector: 'app-new-user',
    templateUrl: './new-user.component.html',
    styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {

    errorMatcher = new InputErrorStateMatcher();

    userForm: FormGroup = new FormGroup({
        username: new FormControl('', [Validators.required]),
        firstName: new FormControl('', [Validators.required]),
        lastName: new FormControl('', [Validators.required]),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required, Validators.minLength(8)]),
        status: new FormControl(0, [Validators.required]),
    });

    constructor(private _service: UserService, private _snackBar: MatSnackBar, private _router: Router) { }

    ngOnInit(): void {
    }

    get userNameControl() {
        return this.userForm.get('username');
    }

    get firstNameControl() {
        return this.userForm.get('firstName');
    }

    get lastNameControl() {
        return this.userForm.get('lastName');
    }

    get emailControl() {
        return this.userForm.get('email');
    }

    get passwordControl() {
        return this.userForm.get('password');
    }

    get statusControl() {
        return this.userForm.get('status');
    }

    onSubmit() {
        const data: User = this.userForm.value;

        this._service.createUser(data).subscribe((response) => {
            if (response.status) {
                this._snackBar.open('User created successfully', undefined, { duration: 5000 });
                this._router.navigate(['/']);
                return;
            }

            this._snackBar.open(response.data as string, undefined, { duration: 5000 });
        })
    }
}
