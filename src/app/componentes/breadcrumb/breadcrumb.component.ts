import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../services/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.scss'
})
export class BreadcrumbComponent {

  breadcrumbs$ = this.breadcrumbService.breadcrumbs$;
  urlAtual: string = '';

  constructor(private breadcrumbService: BreadcrumbService, private router: Router) {
    this.router.events.subscribe(event => {
      this.urlAtual = this.router.url;
    });
  }
}