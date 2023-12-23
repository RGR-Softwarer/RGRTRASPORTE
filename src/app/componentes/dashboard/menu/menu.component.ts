import { Component, ViewChild } from '@angular/core';
import { Sidebar } from 'primeng/sidebar';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent {

  @ViewChild('sidebarRef') sidebarRef!: Sidebar;
  isExpanded: boolean = true;
  sidebarVisible: boolean = true;

  closeCallback(e:Event): void {
      this.sidebarRef.close(e);
  }
  toggleSidebar(): void {
    this.isExpanded = !this.isExpanded;
  }
}
