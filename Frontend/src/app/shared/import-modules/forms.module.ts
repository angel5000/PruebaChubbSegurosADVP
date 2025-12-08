import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ], exports: [
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    
  ]
})
export class FormsModule { }
