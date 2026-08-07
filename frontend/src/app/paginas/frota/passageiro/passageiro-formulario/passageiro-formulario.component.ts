import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PassageiroFacade } from '../services/passageiro.facade';
import { Passageiro } from '../../../../dominio/entidade/passageiro';
import { SexoEnum } from '../../../../dominio/enum/sexo-enum';
import { NotificationService } from '../../../../shared/services/notification.service';
import { FormularioComponent } from '../../../../componentes/formulario/formulario.component';

@Component({
  selector: 'app-passageiro-formulario',
  templateUrl: './passageiro-formulario.component.html',
  styleUrls: ['./passageiro-formulario.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormularioComponent]
})
export class PassageiroFormularioComponent implements OnInit {
  
  passageiroForm!: FormGroup;
  passageiro: Passageiro = new Passageiro();
  isEdicao = false;
  passageiroId?: number;
  isLoading = false;

  // Enum para uso no template
  sexoEnum = SexoEnum;
  sexoOptions = Object.entries(SexoEnum).map(([key, value]) => ({
    key, 
    value, 
    label: value
  }));

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private facade: PassageiroFacade,
    private notificationService: NotificationService
  ) {
    this.inicializarFormulario();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.passageiroId = +params['id'];
        this.isEdicao = true;
        this.carregarPassageiro();
      }
    });
  }

  private inicializarFormulario(): void {
    this.passageiroForm = this.fb.group({
      id: [null],
      nome: ['', [Validators.required, Validators.maxLength(150)]],
      cpf: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(14)]],
      telefone: ['', [Validators.required, Validators.maxLength(15)]],
      email: ['', [Validators.email, Validators.maxLength(100)]],
      sexo: ['', Validators.required],
      localidadeId: ['', Validators.required],
      localidadeEmbarqueId: [''],
      localidadeDesembarqueId: [''],
      observacao: ['', Validators.maxLength(500)],
      situacao: [true]
    });
  }

  private carregarPassageiro(): void {
    if (!this.passageiroId) return;

    this.isLoading = true;
    this.facade.buscarPassageiroPorId(this.passageiroId).subscribe({
      next: (passageiro) => {
        this.passageiro = passageiro;
        this.passageiroForm.patchValue(passageiro);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar passageiro:', error);
        this.notificationService.error('Erro', 'Falha ao carregar dados do passageiro');
        this.isLoading = false;
        this.voltar();
      }
    });
  }

  onSubmit(): void {
    if (this.passageiroForm.valid) {
      this.isLoading = true;
      const passageiroData = { ...this.passageiro, ...this.passageiroForm.value };

      this.facade.salvarPassageiro(passageiroData).subscribe({
        next: (passageiro) => {
          const mensagem = this.isEdicao ? 'Passageiro atualizado com sucesso!' : 'Passageiro criado com sucesso!';
          this.notificationService.success('Sucesso', mensagem);
          this.voltar();
        },
        error: (error) => {
          console.error('Erro ao salvar passageiro:', error);
          this.notificationService.error('Erro', 'Falha ao salvar passageiro');
          this.isLoading = false;
        }
      });
    } else {
      this.marcarCamposComoTocados();
      this.notificationService.warning('Atenção', 'Por favor, preencha todos os campos obrigatórios');
    }
  }

  private marcarCamposComoTocados(): void {
    Object.keys(this.passageiroForm.controls).forEach(key => {
      const control = this.passageiroForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/frota/passageiro']);
  }

  // Método para formatar CPF
  formatarCpf(): void {
    const cpfControl = this.passageiroForm.get('cpf');
    if (cpfControl?.value) {
      let cpf = cpfControl.value.replace(/\D/g, '');
      if (cpf.length <= 11) {
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
        cpfControl.setValue(cpf, { emitEvent: false });
      }
    }
  }

  // Método para formatar telefone
  formatarTelefone(): void {
    const telefoneControl = this.passageiroForm.get('telefone');
    if (telefoneControl?.value) {
      let telefone = telefoneControl.value.replace(/\D/g, '');
      if (telefone.length <= 11) {
        if (telefone.length <= 10) {
          telefone = telefone.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
        } else {
          telefone = telefone.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
        }
        telefoneControl.setValue(telefone, { emitEvent: false });
      }
    }
  }

  // Validar CPF (algoritmo básico)
  validarCpf(): void {
    const cpfControl = this.passageiroForm.get('cpf');
    const cpf = cpfControl?.value?.replace(/\D/g, '');
    
    if (cpf && cpf.length === 11) {
      // Aqui você pode implementar a validação completa do CPF
      // Por simplicidade, vou apenas verificar se não são todos números iguais
      if (/^(\d)\1{10}$/.test(cpf)) {
        cpfControl?.setErrors({ cpfInvalido: true });
        this.notificationService.warning('Atenção', 'CPF inválido');
      } else {
        // Remove erro de CPF inválido se existir
        const errors = cpfControl?.errors;
        if (errors && errors['cpfInvalido']) {
          delete errors['cpfInvalido'];
          cpfControl?.setErrors(Object.keys(errors).length ? errors : null);
        }
      }
    }
  }

  // Buscar localidades (implementação futura)
  buscarLocalidades(termo: string): void {
    // Aqui você pode implementar a busca de localidades
    // Por exemplo, usando um serviço de localidades
    console.log('Buscar localidades:', termo);
  }
}