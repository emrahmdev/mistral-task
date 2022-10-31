import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@NgModule({
    imports: [
        MatTableModule,
        MatInputModule,
        MatSelectModule,
        MatProgressSpinnerModule
    ],
    exports: [
        MatTableModule,
        MatInputModule,
        MatSelectModule,
        MatProgressSpinnerModule
    ]
})
export class ComponentsModule { }
