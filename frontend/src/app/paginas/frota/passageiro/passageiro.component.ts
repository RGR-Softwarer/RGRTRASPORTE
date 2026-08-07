import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Passageiro } from '../../../dominio/entidade/passageiro';
import { ConfiguracaoGrid } from '../../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../../dominio/interface/grid/action-grid';
import { Router } from '@angular/router';
import { PassageiroFacade } from './services/passageiro.facade';
import { Observable } from 'rxjs';
import { DecoratorUtils } from '../../../services/decorator/formulario-decorator';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfigService } from '../../../services/config/config.service';
import { GridComponent } from '../../../componentes/grid/grid.component';

@Component({
  selector: 'app-passageiro',
  templateUrl: './passageiro.component.html',
  styleUrls: ['./passageiro.component.scss'],
  standalone: true,
  imports: [CommonModule, GridComponent]
})
export class PassageiroComponent implements OnInit {

  // Propriedades para o GridComponent
  buscarTodosUrl: string;
  adicionarUrl: string = '/frota/passageiro/adicionar';
  entidade: any = new Passageiro();
  identificador: string = 'passageiro-grid';

  // Observables da facade (para uso futuro se necessário)
  passageiros$: Observable<Passageiro[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private router: Router,
    private facade: PassageiroFacade,
    private notificationService: NotificationService,
    private configService: ConfigService
  ) {
    this.passageiros$ = this.facade.passageiros$;
    this.isLoading$ = this.facade.isLoading$;
    this.buscarTodosUrl = `${this.configService.getApiBaseUrl()}/Passageiro`;
  }

  ngOnInit(): void {
    // O GridComponent fará sua própria chamada de API
    // Não precisamos chamar facade.carregarPassageiros() aqui
  }

  /**
   * Navega para a página de adicionar passageiro
   */
  adicionar(): void {
    this.router.navigate(['/frota/passageiro/adicionar']);
  }

  /**
   * Navega para a página de editar passageiro
   * @param id ID do passageiro a ser editado
   */
  editar(id: number): void {
    this.router.navigate([`/frota/passageiro/editar/${id}`]);
  }

  /**
   * Deleta um passageiro após confirmação
   * @param id ID do passageiro a ser deletado
   */
  deletar(id: number): void {
    if (confirm('Tem certeza que deseja deletar este passageiro?')) {
        this.facade.deletarPassageiro(id);
    }
  }

  /**
   * Recarrega os dados da grid (pode ser usado para refresh manual)
   */
  recarregarDados(): void {
    this.facade.carregarPassageiros();
    this.notificationService.success('Sucesso', 'Dados recarregados com sucesso!');
  }
  
}