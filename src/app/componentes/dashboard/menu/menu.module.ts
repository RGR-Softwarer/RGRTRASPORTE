import { AvatarModule } from 'primeng/avatar';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { MenuComponent } from './menu.component';



@NgModule({
  declarations: [
    MenuComponent
  ],
  imports: [
    CommonModule,
    SidebarModule,
    ButtonModule,
    AvatarModule   
  ],
  exports: [
    MenuComponent
  ]  
})
export class MenuModule { }
