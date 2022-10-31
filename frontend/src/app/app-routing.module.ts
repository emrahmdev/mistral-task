import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditUserComponent } from './pages/edit-user/edit-user.component';
import { NewUserComponent } from './pages/new-user/new-user.component';
import { UsersComponent } from './pages/users/users.component';

const routes: Routes = [
	{
		path: '',
		component: UsersComponent
	},
	{
		path: 'user/new',
		component: NewUserComponent
	},
	{
		path: 'user/:userId',
		component: UsersComponent
	},
	{
		path: 'user/:userId/edit',
		component: EditUserComponent
	}
];

@NgModule({
	imports: [RouterModule.forRoot(routes, { useHash: true })],
	exports: [RouterModule]
})
export class AppRoutingModule { }
