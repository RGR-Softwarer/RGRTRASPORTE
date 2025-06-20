import { Component, OnInit } from '@angular/core';
import { Veiculo } from '../../../dominio/entidade/veiculo';
import { ConfiguracaoGrid } from '../../../dominio/interface/grid/configuracao-grid';
import { Action } from '../../../dominio/interface/grid/action-grid';
import { Router } from '@angular/router';
import { VeiculoFacade } from './services/veiculo.facade';
import { Observable } from 'rxjs';
import { DecoratorUtils } from '../../../services/decorator/formulario-decorator';
import { ToastService } from '../../../services/utils/notificacao/toast.service';

@Component({
  selector: 'app-veiculo',
  templateUrl: './veiculo.component.html',
  styleUrls: ['./veiculo.component.scss'],
})
export class VeiculoComponent implements OnInit {

  // Propriedades para o GridComponent
  buscarTodosUrl: string = '/api/Veiculo';
  adicionarUrl: string = '/frota/veiculo/adicionar';
  entidade: any = new Veiculo();
  identificador: string = 'veiculo-grid';

  // Observables da facade (para uso futuro se necessário)
  veiculos$: Observable<Veiculo[]>;
  isLoading$: Observable<boolean>;

  constructor(
    private router: Router,
    private facade: VeiculoFacade,
    private toast: ToastService
  ) {
    this.veiculos$ = this.facade.veiculos$;
    this.isLoading$ = this.facade.isLoading$;
  }

  ngOnInit(): void {
    // O GridComponent fará sua própria chamada de API
    // Não precisamos chamar facade.carregarVeiculos() aqui
  }

  /**
   * Navega para a página de adicionar veículo
   */
  adicionar(): void {
    this.router.navigate(['/frota/veiculo/adicionar']);
  }

  /**
   * Navega para a página de editar veículo
   * @param id ID do veículo a ser editado
   */
  editar(id: number): void {
    this.router.navigate([`/frota/veiculo/editar/${id}`]);
  }

  /**
   * Deleta um veículo após confirmação
   * @param id ID do veículo a ser deletado
   */
  deletar(id: number): void {
    if (confirm('Tem certeza que deseja deletar este veículo?')) {
        this.facade.deletarVeiculo(id);
    }
  }

  /**
   * Recarrega os dados da grid (pode ser usado para refresh manual)
   */
  recarregarDados(): void {
    this.facade.carregarVeiculos();
    this.toast.exibirMensagemSucesso('Sucesso', 'Dados recarregados com sucesso!');
  }
}