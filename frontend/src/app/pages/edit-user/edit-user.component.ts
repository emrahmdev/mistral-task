import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, of } from 'rxjs';
import { User } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';
import { InputErrorStateMatcher } from 'src/app/core/utils/input-error-state-matcher';

@Component({
    selector: 'app-edit-user',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {

    errorMatcher = new InputErrorStateMatcher();

    userForm: FormGroup = new FormGroup({
        userId: new FormControl(0, [Validators.required]),
        firstName: new FormControl('', [Validators.required]),
        lastName: new FormControl('', [Validators.required]),
        email: new FormControl('', [Validators.required, Validators.email]),
        status: new FormControl(0, [Validators.required]),
    });

    private userId: number = -1;

    constructor(private _service: UserService, private _snackBar: MatSnackBar, private _router: Router, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this._route.paramMap.subscribe(params => {
            this.userId = parseInt(params.get('userId') || "-1");

            this._service.getUserById(this.userId)
            .pipe(catchError(() => of(null)))
            .subscribe(response => {
                if(response == null || !response.status) {
                    this._router.navigate(['/']);
                    return;
                }

                const user: User = response.data as User;
                this.userForm.patchValue(user);
            });
        });
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

    get statusControl() {
        return this.userForm.get('status');
    }

    onSubmit() {
        const data: User = this.userForm.value;

        this._service.updateUser(this.userId, data).subscribe((response) => {
            if (response.status) {
                this._snackBar.open('User updated successfully', undefined, { duration: 5 });
                this._router.navigate(['/']);
                return;
            }

            this._snackBar.open(response.data as string, undefined, { duration: 5 });
        })
    }

}
