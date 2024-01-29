import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from './grid.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzButtonModule } from 'ng-zorro-antd/button';

@NgModule({
  declarations: [GridComponent],
  imports: [
    CommonModule,
    NzTableModule,
    ReactiveFormsModule,
    NzFormModule,
    NzDividerModule,
    NzRadioModule,
    NzSwitchModule,
    NzButtonModule
  ],
  exports: [GridComponent]
})
export class GridModule { }