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
  
    filhos.forEach(filho => {
      const segmentosRota = filho.snapshot.url.map(segmento => segmento.path);
  
      segmentosRota.forEach((segmento, index) => {
        if (segmento) {
          url += `/${segmento}`;
  
          let rotulo = this.primeiraLetraMaiscula(segmento);
          if (index === segmentosRota.length - 1) {
            rotulo = filho.snapshot.data['breadcrumb'] ? this.primeiraLetraMaiscula(filho.snapshot.data['breadcrumb']) : rotulo;
          }

          breadcrumbs.push({ label: rotulo, url });
        }
      });
  
      this.criarBreadcrumbs(filho, url, breadcrumbs);
    });
  
    return breadcrumbs;
  }
  
  private primeiraLetraMaiscula(s: string): string {
    return s.charAt(0).toUpperCase() + s.slice(1);
  }   
  
}