import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Viagem } from '../../../dominio/entidade/viagem';
import { ConfiguracaoGrid } from '../../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../../dominio/interface/grid/action-grid';
import { Router } from '@angular/router';
import { ViagemFacade } from './services/viagem.facade';
import { Observable } from 'rxjs';
import { DecoratorUtils } from '../../../services/decorator/formulario-decorator';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfigService } from '../../../services/config/config.service';
import { GridComponent } from '../../../componentes/grid/grid.component';

@Component({
  selector: 'app-viagem',
  templateUrl: './viagem.component.html',
  styleUrls: ['./viagem.component.scss'],
  standalone: true,
  imports: [CommonModule, GridComponent]
})
export class ViagemComponent implements OnInit {

  // Propriedades para o GridComponent
  buscarTodosUrl: string;
  adicionarUrl: string = '/frota/viagem/adicionar';
  entidade: any = new Viagem();
  identificador: string = 'viagem-grid';

  // Observables da facade (para uso futuro se necessário)
  viagens$: Observable<Viagem[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private router: Router,
    private facade: ViagemFacade,
    private notificationService: NotificationService,
    private configService: ConfigService
  ) {
    this.viagens$ = this.facade.viagens$;
    this.isLoading$ = this.facade.isLoading$;
    this.buscarTodosUrl = `${this.configService.getApiBaseUrl()}/Viagem`;
  }

  ngOnInit(): void {
    // O GridComponent fará sua própria chamada de API
    // Não precisamos chamar facade.carregarViagens() aqui
  }

  /**
   * Navega para a página de adicionar viagem
   */
  adicionar(): void {
    this.router.navigate(['/frota/viagem/adicionar']);
  }

  /**
   * Navega para a página de editar viagem
   * @param id ID da viagem a ser editada
   */
  editar(id: number): void {
    this.router.navigate([`/frota/viagem/editar/${id}`]);
  }

  /**
   * Visualiza detalhes da viagem
   * @param id ID da viagem
   */
  visualizar(id: number): void {
    this.router.navigate([`/frota/viagem/detalhes/${id}`]);
  }

  /**
   * Cancela uma viagem
   * @param id ID da viagem a ser cancelada
   */
  cancelar(id: number): void {
    const motivo = prompt('Informe o motivo do cancelamento (opcional):');
    if (motivo !== null) { // null significa que o usuário cancelou o prompt
      this.facade.cancelarViagem(id, motivo || undefined);
    }
  }

  /**
   * Inicia uma viagem
   * @param id ID da viagem a ser iniciada
   */
  iniciar(id: number): void {
    this.facade.iniciarViagem(id);
  }

  /**
   * Finaliza uma viagem
   * @param id ID da viagem a ser finalizada
   */
  finalizar(id: number): void {
    this.facade.finalizarViagem(id);
  }

  /**
   * Visualiza a rota da viagem
   * @param id ID da viagem
   */
  verRota(id: number): void {
    this.facade.obterRotaViagem(id).subscribe({
      next: (rota) => {
        if (rota) {
          // Aqui você pode abrir um modal ou navegar para uma página de visualização da rota
          console.log('Rota da viagem:', rota);
          this.notificationService.info('Rota', 'Dados da rota carregados no console');
        }
      },
      error: (error) => {
        console.error('Erro ao obter rota:', error);
      }
    });
  }

  /**
   * Gerencia passageiros da viagem
   * @param id ID da viagem
   */
  gerenciarPassageiros(id: number): void {
    this.router.navigate([`/frota/viagem/${id}/passageiros`]);
  }

  /**
   * Visualiza posições da viagem
   * @param id ID da viagem
   */
  verPosicoes(id: number): void {
    this.router.navigate([`/frota/viagem/${id}/posicoes`]);
  }

  /**
   * Recarrega os dados da grid (pode ser usado para refresh manual)
   */
  recarregarDados(): void {
    this.facade.carregarViagens();
    this.notificationService.success('Sucesso', 'Dados recarregados com sucesso!');
  }
  
}