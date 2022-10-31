import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/user.model';

@Component({
	selector: 'app-users',
	templateUrl: './users.component.html',
	styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

	displayedColumns: string[] = ['userName', 'firstName', 'lastName', 'email', 'status'];
	dataSource: User[] = [
		{
			firstName: 'emrah',
			lastName: 'malikic',
			userName: 'emrah.m',
			email: 'emrah.m.dev@gmail.com',
			status: 1,
			password: ''
		}
	];

	isLoading: boolean = true;

	constructor() { }

	ngOnInit(): void {
	}

}
