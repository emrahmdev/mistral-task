import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { BehaviorSubject, catchError, debounceTime, distinctUntilChanged, fromEvent, map, merge, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { PaginatedResponse } from 'src/app/core/models/paginated-response.model';
import { User } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';
import { ModalComponent } from 'src/app/core/shared/components/modal/modal.component';

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, AfterViewInit {

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
    @ViewChild(MatSort, { static: false }) sort: MatSort | undefined;
    @ViewChild('filterInput') filterInput: ElementRef | undefined;

    displayedColumns: string[] = ['username', 'firstName', 'lastName', 'email', 'status', 'edit', 'permissions', 'delete'];
    filters: string[] = ['username', 'firstName', 'lastName', 'email', 'status']
    isLoading: boolean = true;

    private refreshData: Subject<void> = new Subject();
    refreshData$: Observable<void> = this.refreshData.asObservable();

    private selectedFilter: BehaviorSubject<string> = new BehaviorSubject("all");
    selectedFilter$: Observable<string> = this.selectedFilter.asObservable();

    private _dataSource: PaginatedResponse<User> | undefined;

    get dataSource(): User[] {
        return this._dataSource?.items || [];
    }

    get pageLength() {
        return (this._dataSource?.totalPages || 0) * (this.paginator?.pageSize || 10);
    }

    constructor(private _service: UserService, public _dialog: MatDialog, private _snackBar: MatSnackBar) { }

    ngOnInit(): void {
    }

    ngAfterViewInit(): void {
        this.sort?.sortChange.subscribe(() => {
            if (this.paginator != undefined) {
                this.paginator.pageIndex = 0
            }
        });

        if (this.paginator != undefined && this.sort != undefined && this.filterInput != undefined) {

            const filterText$ = fromEvent(this.filterInput.nativeElement, 'keyup')
                .pipe(
                    debounceTime(400),
                    distinctUntilChanged(),
                );

            merge(this.sort?.sortChange, this.paginator.page, this.selectedFilter$, filterText$, this.refreshData$)
                .pipe(
                    debounceTime(400),
                    startWith({}),
                    switchMap(() => {
                        this.isLoading = true;
                        return this._service.getAllUsers(
                            (this.paginator?.pageIndex || 0) + 1,
                            this.paginator?.pageSize || 10,
                            this.sort?.active || "",
                            this.sort?.direction || "",
                            this.selectedFilter.value,
                            this.filterInput?.nativeElement.value || ""
                        ).pipe(catchError(() => of(null)));
                    }),
                    map(response => {
                        if (!response?.status) {
                            return null;
                        }

                        return response.data as PaginatedResponse<User>;
                    })
                )
                .subscribe(data => {
                    this.isLoading = false;

                    if (data != null) {
                        this._dataSource = data;
                    }
                });
        }
    }

    onFilterColumnChange(e: string) {
        this.selectedFilter.next(e);
    }

    onDeleteClick(userId: number) {
        this._dialog.open(ModalComponent, {
            data: { message: 'Are you sure you want to delete this user' }
        }).afterClosed().subscribe(result => {
            if (result) {
                this._service.deleteUser(userId)
                    .subscribe((status: boolean) => {
                        if (!status) {
                            this._snackBar.open('Failed to delete user', undefined, { duration: 5000 });
                            return;
                        }
                        this._snackBar.open('User deleted successfully', undefined, { duration: 5000 });
                        this.refreshData.next();
                    })
            }
        })
    }
}
