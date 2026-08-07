import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { InputFieldComponent } from '../../shared/components/form/input-field/input-field.component';
import { SelectFieldComponent } from '../../shared/components/form/select-field/select-field.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    InputFieldComponent,
    SelectFieldComponent
  ]
})
export class GridModule { }