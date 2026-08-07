import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModeloVeicular } from '../../../dominio/entidade/veiculo/modelo-veicular';
import { ConfiguracaoGrid } from '../../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../../dominio/interface/grid/action-grid';
import { Router } from '@angular/router';
import { ModeloVeicularFacade } from './services/modelo-veicular.facade';
import { Observable } from 'rxjs';
import { DecoratorUtils } from '../../../services/decorator/formulario-decorator';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfigService } from '../../../services/config/config.service';
import { GridComponent } from '../../../componentes/grid/grid.component';

@Component({
  selector: 'app-modelo-veicular',
  templateUrl: './modelo-veicular.component.html',
  styleUrls: ['./modelo-veicular.component.scss'],
  standalone: true,
  imports: [CommonModule, GridComponent]
})
export class ModeloVeicularComponent implements OnInit {

  // Propriedades para o GridComponent
  buscarTodosUrl: string;
  adicionarUrl: string = '/frota/modelo-veicular/adicionar';
  entidade: any = new ModeloVeicular();
  identificador: string = 'modelo-veicular-grid';

  // Observables da facade (para uso futuro se necessário)
  modelos$: Observable<ModeloVeicular[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private router: Router,
    private facade: ModeloVeicularFacade,
    private notificationService: NotificationService,
    private configService: ConfigService
  ) {
    this.modelos$ = this.facade.modelos$;
    this.isLoading$ = this.facade.isLoading$;
    this.buscarTodosUrl = `${this.configService.getApiBaseUrl()}/ModeloVeicular`;
  }

  ngOnInit(): void {
    // O GridComponent fará sua própria chamada de API
    // Não precisamos chamar facade.carregarModelos() aqui
  }

  /**
   * Navega para a página de adicionar modelo veicular
   */
  adicionar(): void {
    this.router.navigate(['/frota/modelo-veicular/adicionar']);
  }

  /**
   * Navega para a página de editar modelo veicular
   * @param id ID do modelo veicular a ser editado
   */
  editar(id: number): void {
    this.router.navigate([`/frota/modelo-veicular/editar/${id}`]);
  }

  /**
   * Deleta um modelo veicular após confirmação
   * @param id ID do modelo veicular a ser deletado
   */
  deletar(id: number): void {
    if (confirm('Tem certeza que deseja deletar este modelo veicular?')) {
        this.facade.deletarModelo(id);
    }
  }

  /**
   * Popula dados iniciais (seed)
   */
  popularDadosIniciais(): void {
    if (confirm('Deseja popular os dados iniciais de modelos veiculares? Esta ação pode sobrescrever dados existentes.')) {
      this.facade.popularDadosIniciais(false);
    }
  }

  /**
   * Força a recriação dos dados iniciais
   */
  recriarDadosIniciais(): void {
    if (confirm('Deseja recriar todos os dados iniciais? Esta ação irá remover todos os dados existentes e criar novos.')) {
      this.facade.popularDadosIniciais(true);
    }
  }

  /**
   * Recarrega os dados da grid (pode ser usado para refresh manual)
   */
  recarregarDados(): void {
    this.facade.carregarModelos();
    this.notificationService.success('Sucesso', 'Dados recarregados com sucesso!');
  }
  
}