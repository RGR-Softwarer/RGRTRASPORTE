import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModeloVeicularFacade } from '../services/modelo-veicular.facade';
import { ModeloVeicular } from '../../../../dominio/entidade/veiculo/modelo-veicular';
import { TipoModeloVeiculoEnum } from '../../../../dominio/enum/veiculo/tipo-modelo-veiculo-enum';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';

@Component({
  selector: 'app-modelo-veicular-formulario',
  templateUrl: './modelo-veicular-formulario.component.html',
  styleUrls: ['./modelo-veicular-formulario.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormularioComponent]
})
export class ModeloVeicularFormularioComponent implements OnInit {
  
  modeloForm!: FormGroup;
  modelo: ModeloVeicular = new ModeloVeicular();
  isEdicao = false;
  modeloId?: number;
  isLoading = false;

  // Enum para uso no template
  tipoModeloVeiculoEnum = TipoModeloVeiculoEnum;
  tipoModeloOptions = Object.entries(TipoModeloVeiculoEnum).map(([key, value]) => ({
    key, 
    value, 
    label: this.getTipoLabel(value)
  }));

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private facade: ModeloVeicularFacade,
    private notificationService: NotificationService
  ) {
    this.inicializarFormulario();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.modeloId = +params['id'];
        this.isEdicao = true;
        this.carregarModelo();
      }
    });
  }

  private inicializarFormulario(): void {
    this.modeloForm = this.fb.group({
      id: [null],
      descricao: ['', [Validators.required, Validators.maxLength(200)]],
      situacao: [true, Validators.required],
      tipo: ['', Validators.required],
      quantidadeAssento: [0, [Validators.required, Validators.min(1), Validators.max(100)]],
      quantidadeEixo: [2, [Validators.required, Validators.min(1), Validators.max(10)]],
      capacidadeMaxima: [0, [Validators.required, Validators.min(1), Validators.max(200)]],
      passageirosEmPe: [0, [Validators.min(0), Validators.max(50)]],
      possuiBanheiro: [false],
      possuiClimatizador: [false]
    });

    // Listener para calcular capacidade máxima automaticamente
    this.modeloForm.get('quantidadeAssento')?.valueChanges.subscribe(assentos => {
      const passageirosEmPe = this.modeloForm.get('passageirosEmPe')?.value || 0;
      if (assentos) {
        this.modeloForm.patchValue({
          capacidadeMaxima: assentos + passageirosEmPe
        }, { emitEvent: false });
      }
    });

    this.modeloForm.get('passageirosEmPe')?.valueChanges.subscribe(emPe => {
      const assentos = this.modeloForm.get('quantidadeAssento')?.value || 0;
      if (emPe !== null) {
        this.modeloForm.patchValue({
          capacidadeMaxima: assentos + emPe
        }, { emitEvent: false });
      }
    });
  }

  private carregarModelo(): void {
    if (!this.modeloId) return;

    this.isLoading = true;
    this.facade.buscarModeloPorId(this.modeloId).subscribe({
      next: (modelo) => {
        this.modelo = modelo;
        this.modeloForm.patchValue(modelo);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar modelo veicular:', error);
        this.notificationService.error('Erro', 'Falha ao carregar dados do modelo veicular');
        this.isLoading = false;
        this.voltar();
      }
    });
  }

  onSubmit(): void {
    if (this.modeloForm.valid) {
      this.isLoading = true;
      const modeloData = { ...this.modelo, ...this.modeloForm.value };

      this.facade.salvarModelo(modeloData).subscribe({
        next: (modelo) => {
          const mensagem = this.isEdicao ? 'Modelo veicular atualizado com sucesso!' : 'Modelo veicular criado com sucesso!';
          this.notificationService.success('Sucesso', mensagem);
          this.voltar();
        },
        error: (error) => {
          console.error('Erro ao salvar modelo veicular:', error);
          this.notificationService.error('Erro', 'Falha ao salvar modelo veicular');
          this.isLoading = false;
        }
      });
    } else {
      this.marcarCamposComoTocados();
      this.notificationService.warning('Atenção', 'Por favor, preencha todos os campos obrigatórios');
    }
  }

  private marcarCamposComoTocados(): void {
    Object.keys(this.modeloForm.controls).forEach(key => {
      const control = this.modeloForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/frota/modelo-veicular']);
  }

  private getTipoLabel(tipo: TipoModeloVeiculoEnum): string {
    const labels: { [key in TipoModeloVeiculoEnum]: string } = {
      [TipoModeloVeiculoEnum.Onibus]: 'Ônibus',
      [TipoModeloVeiculoEnum.Microonibus]: 'Micro-ônibus',
      [TipoModeloVeiculoEnum.Van]: 'Van',
      [TipoModeloVeiculoEnum.Carro]: 'Carro'
    };
    return labels[tipo] || tipo;
  }

  /**
   * Define valores padrão baseados no tipo selecionado
   */
  onTipoChange(): void {
    const tipo = this.modeloForm.get('tipo')?.value;
    if (!tipo) return;

    // Definir valores padrão baseados no tipo
    const defaults = this.getDefaultValuesByType(tipo);
    this.modeloForm.patchValue(defaults, { emitEvent: true });
  }

  private getDefaultValuesByType(tipo: TipoModeloVeiculoEnum): Partial<ModeloVeicular> {
    switch (tipo) {
      case TipoModeloVeiculoEnum.Onibus:
        return {
          quantidadeAssento: 40,
          quantidadeEixo: 3,
          passageirosEmPe: 20,
          possuiBanheiro: true,
          possuiClimatizador: true
        };
      case TipoModeloVeiculoEnum.Microonibus:
        return {
          quantidadeAssento: 20,
          quantidadeEixo: 2,
          passageirosEmPe: 5,
          possuiBanheiro: false,
          possuiClimatizador: true
        };
      case TipoModeloVeiculoEnum.Van:
        return {
          quantidadeAssento: 15,
          quantidadeEixo: 2,
          passageirosEmPe: 0,
          possuiBanheiro: false,
          possuiClimatizador: true
        };
      case TipoModeloVeiculoEnum.Carro:
        return {
          quantidadeAssento: 5,
          quantidadeEixo: 2,
          passageirosEmPe: 0,
          possuiBanheiro: false,
          possuiClimatizador: true
        };
      default:
        return {};
    }
  }
}