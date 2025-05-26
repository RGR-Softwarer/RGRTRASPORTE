import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormularioComponent } from './formulario.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { RouterModule } from '@angular/router';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [FormularioComponent],
  imports: [
    CommonModule,
    RouterModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    ReactiveFormsModule,
    FormsModule,
    NzSelectModule,
    NzIconModule
  ],
  exports: [
    FormularioComponent
  ]
})
export class FormularioModule { }
