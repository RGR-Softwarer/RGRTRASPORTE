import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormularioComponent } from './formulario.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms'; // Import necessário
import {NzSelectModule} from 'ng-zorro-antd/select';

@NgModule({
  declarations: [FormularioComponent],
  imports: [
    CommonModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    ReactiveFormsModule,
    NzSelectModule
  ],
  exports: [
    FormularioComponent,
  ]
})
export class FormularioModule { }
