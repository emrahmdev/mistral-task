import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin, map } from 'rxjs';
import { UserPermissions } from 'src/app/core/models/user-permissions.model';
import { UserService } from 'src/app/core/services/user.service';

@Component({
    selector: 'app-permissions',
    templateUrl: './permissions.component.html',
    styleUrls: ['./permissions.component.scss']
})
export class PermissionsComponent implements OnInit {
    displayedColumns: string[] = ['name', 'status'];
    dataSource: UserPermissions[] = [];

    private userId: number = -1;

    constructor(private _service: UserService, private _route: ActivatedRoute) { }

    ngOnInit(): void {
        this._route.paramMap.subscribe(params => {
            this.userId = parseInt(params.get('userId') || "-1");

            forkJoin({
                userPermission: this._service.getUserPermissions(this.userId),
                allPermissions: this._service.getAllPermissions()
            })
            .pipe(
                map(results => {
                    const userPermission: UserPermissions[] = [];

                    results.allPermissions.forEach(permission => {
                        const active: boolean = results.userPermission.some(up => up.permissionId == permission.permissionId);

                        const newPermission: UserPermissions = {
                            permissionId: permission.permissionId,
                            name: permission.name,
                            active: active
                        }

                        userPermission.push(newPermission);
                    })

                    return userPermission;
                })
            )
            .subscribe(userPermission => {
                this.dataSource = userPermission;
            });
        });
    }

    onPermissionChange(permission: UserPermissions) {
        if(permission.active) {
            this._service.assignUserPermission(this.userId, permission.permissionId).subscribe(result => {
                permission.active = result;
            });
            return;
        }

        this._service.revokeUserPermission(this.userId, permission.permissionId).subscribe(result => {
            permission.active = !result;
        });
    }
}
