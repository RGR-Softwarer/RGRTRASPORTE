import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from './grid.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

@NgModule({
  declarations: [GridComponent],
  imports: [
    CommonModule,
    NzTableModule,
    ReactiveFormsModule,
    FormsModule,
    NzFormModule,
    NzDividerModule,
    NzRadioModule,
    NzSwitchModule,
    NzButtonModule,
    NzModalModule,
    NzCheckboxModule,
    NzIconModule,
    NzToolTipModule
  ],
  exports: [GridComponent]
})
export class GridModule { }