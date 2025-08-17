import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LocalidadeFacade } from '../services/localidade.facade';
import { Localidade } from '../../../../dominio/entidade/localidade';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';

@Component({
  selector: 'app-localidade-formulario',
  templateUrl: './localidade-formulario.component.html',
  styleUrls: ['./localidade-formulario.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormularioComponent]
})
export class LocalidadeFormularioComponent implements OnInit {
  
  localidadeForm!: FormGroup;
  localidade: Localidade = new Localidade();
  isEdicao = false;
  localidadeId?: number;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private facade: LocalidadeFacade,
    private notificationService: NotificationService
  ) {
    this.inicializarFormulario();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.localidadeId = +params['id'];
        this.isEdicao = true;
        this.carregarLocalidade();
      }
    });
  }

  private inicializarFormulario(): void {
    this.localidadeForm = this.fb.group({
      id: [null],
      nome: ['', [Validators.required, Validators.maxLength(100)]],
      estado: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(2)]],
      cidade: ['', [Validators.required, Validators.maxLength(100)]],
      cep: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(9)]],
      bairro: ['', [Validators.required, Validators.maxLength(100)]],
      logradouro: ['', [Validators.required, Validators.maxLength(200)]],
      numero: ['', Validators.maxLength(10)],
      complemento: ['', Validators.maxLength(100)],
      latitude: ['', [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: ['', [Validators.required, Validators.min(-180), Validators.max(180)]],
      ativo: [true]
    });
  }

  private carregarLocalidade(): void {
    if (!this.localidadeId) return;

    this.isLoading = true;
    this.facade.buscarLocalidadePorId(this.localidadeId).subscribe({
      next: (localidade) => {
        this.localidade = localidade;
        this.localidadeForm.patchValue(localidade);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar localidade:', error);
        this.notificationService.error('Erro', 'Falha ao carregar dados da localidade');
        this.isLoading = false;
        this.voltar();
      }
    });
  }

  onSubmit(): void {
    if (this.localidadeForm.valid) {
      this.isLoading = true;
      const localidadeData = { ...this.localidade, ...this.localidadeForm.value };

      this.facade.salvarLocalidade(localidadeData).subscribe({
        next: (localidade) => {
          const mensagem = this.isEdicao ? 'Localidade atualizada com sucesso!' : 'Localidade criada com sucesso!';
          this.notificationService.success('Sucesso', mensagem);
          this.voltar();
        },
        error: (error) => {
          console.error('Erro ao salvar localidade:', error);
          this.notificationService.error('Erro', 'Falha ao salvar localidade');
          this.isLoading = false;
        }
      });
    } else {
      this.marcarCamposComoTocados();
      this.notificationService.warning('Atenção', 'Por favor, preencha todos os campos obrigatórios');
    }
  }

  private marcarCamposComoTocados(): void {
    Object.keys(this.localidadeForm.controls).forEach(key => {
      const control = this.localidadeForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/frota/localidade']);
  }

  // Método para buscar CEP (integração futura com API de CEP)
  buscarCep(): void {
    const cep = this.localidadeForm.get('cep')?.value;
    if (cep && cep.length >= 8) {
      // Aqui você pode integrar com uma API de CEP como ViaCEP
      console.log('Buscar CEP:', cep);
      // Exemplo de implementação futura:
      // this.cepService.buscarCep(cep).subscribe(dados => {
      //   this.localidadeForm.patchValue({
      //     cidade: dados.localidade,
      //     estado: dados.uf,
      //     bairro: dados.bairro,
      //     logradouro: dados.logradouro
      //   });
      // });
    }
  }

  // Método para obter localização atual (GPS)
  obterLocalizacaoAtual(): void {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          this.localidadeForm.patchValue({
            latitude: position.coords.latitude,
            longitude: position.coords.longitude
          });
          this.notificationService.success('Sucesso', 'Localização obtida com sucesso!');
        },
        (error) => {
          console.error('Erro ao obter localização:', error);
          this.notificationService.error('Erro', 'Não foi possível obter a localização atual');
        }
      );
    } else {
      this.notificationService.error('Erro', 'Geolocalização não é suportada pelo navegador');
    }
  }
}