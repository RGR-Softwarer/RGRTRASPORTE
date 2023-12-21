import { Component, OnInit } from '@angular/core';
import { AppContext } from '../../../dominio/entidade/app.context';
import { AppContextService } from '../../../services/context/app.context';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  userContext: AppContext | null = null;

  constructor(private appContextService: AppContextService) { }

  ngOnInit() {
    this.userContext = this.appContextService.obterUsuarioLogado();
    console.log(this.userContext);
  }
}