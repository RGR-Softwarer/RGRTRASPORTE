import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { ItemBreadcrumb } from '../../dominio/interface/ItemBreadcrumb';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  private breadcrumbs = new BehaviorSubject<ItemBreadcrumb[]>([]);
  breadcrumbs$ = this.breadcrumbs.asObservable();

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      const raiz = this.activatedRoute.root;
      const breadcrumbs: ItemBreadcrumb[] = this.criarBreadcrumbs(raiz);
      this.breadcrumbs.next(breadcrumbs);
    });
  }

  private criarBreadcrumbs(rota: ActivatedRoute, url: string = '', breadcrumbs: ItemBreadcrumb[] = []): ItemBreadcrumb[] {
    const filhos: ActivatedRoute[] = rota.children;

    if (filhos.length === 0) {
      return breadcrumbs;
    }

    for (const filho of filhos) {
      const urlRota: string = filho.snapshot.url.map(segmento => segmento.path).join('/');
      if (urlRota !== '') {
        url += `/${urlRota}`;
      }

      const rotulo = filho.snapshot.data['breadcrumb'];
      if (rotulo) {
        breadcrumbs.push({ label: rotulo, url });
      }

      return this.criarBreadcrumbs(filho, url, breadcrumbs);
    }

    return breadcrumbs;
  }
}