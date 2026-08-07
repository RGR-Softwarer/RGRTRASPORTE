import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Localidade } from '../../../dominio/entidade/localidade';
import { ConfiguracaoGrid } from '../../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../../dominio/interface/grid/action-grid';
import { Router } from '@angular/router';
import { LocalidadeFacade } from './services/localidade.facade';
import { Observable } from 'rxjs';
import { DecoratorUtils } from '../../../services/decorator/formulario-decorator';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfigService } from '../../../services/config/config.service';
import { GridComponent } from '../../../componentes/grid/grid.component';

@Component({
  selector: 'app-localidade',
  templateUrl: './localidade.component.html',
  styleUrls: ['./localidade.component.scss'],
  standalone: true,
  imports: [CommonModule, GridComponent]
})
export class LocalidadeComponent implements OnInit {

  // Propriedades para o GridComponent
  buscarTodosUrl: string;
  adicionarUrl: string = '/frota/localidade/adicionar';
  entidade: any = new Localidade();
  identificador: string = 'localidade-grid';

  // Observables da facade (para uso futuro se necessário)
  localidades$: Observable<Localidade[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private router: Router,
    private facade: LocalidadeFacade,
    private notificationService: NotificationService,
    private configService: ConfigService
  ) {
    this.localidades$ = this.facade.localidades$;
    this.isLoading$ = this.facade.isLoading$;
    this.buscarTodosUrl = `${this.configService.getApiBaseUrl()}/Localidade`;
  }

  ngOnInit(): void {
    // O GridComponent fará sua própria chamada de API
    // Não precisamos chamar facade.carregarLocalidades() aqui
  }

  /**
   * Navega para a página de adicionar localidade
   */
  adicionar(): void {
    this.router.navigate(['/frota/localidade/adicionar']);
  }

  /**
   * Navega para a página de editar localidade
   * @param id ID da localidade a ser editada
   */
  editar(id: number): void {
    this.router.navigate([`/frota/localidade/editar/${id}`]);
  }

  /**
   * Deleta uma localidade após confirmação
   * @param id ID da localidade a ser deletada
   */
  deletar(id: number): void {
    if (confirm('Tem certeza que deseja deletar esta localidade?')) {
        this.facade.deletarLocalidade(id);
    }
  }

  /**
   * Recarrega os dados da grid (pode ser usado para refresh manual)
   */
  recarregarDados(): void {
    this.facade.carregarLocalidades();
    this.notificationService.success('Sucesso', 'Dados recarregados com sucesso!');
  }
  
}