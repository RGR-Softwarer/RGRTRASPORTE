import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router'; // Importação do Router
import { Location } from '@angular/common'; // Importação do Location
import { FormCampoConstrutor, FormCamposMetadata } from '../../services/decorator/formulario-decorator';
import { ToastService } from '../../services/utils/notificacao/toast.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styleUrls: ['./formulario.component.scss']
})
export class FormularioComponent implements OnInit, AfterViewInit {
  @Input() salvarCallback: (data: any) => void = () => {};
  @Input() objeto: any;

  form: FormGroup = new FormGroup({});
  formFields: FormCamposMetadata[] = [];
  dados: any; // Dados extraídos da rota para edição
  isEditMode: boolean = false; // Define se é edição ou adição

  constructor(
    private fb: FormBuilder, 
    private toastService: ToastService,
    private route: ActivatedRoute,
    private cd: ChangeDetectorRef,
    private router: Router, // Injeção do Router
    private location: Location // Injeção do Location
  ) {}

  ngOnInit() {
    this.inicializarFormulario();
  }

  inicializarFormulario() {
    // Inicializa `formFields` a partir do construtor do objeto
    const objetoConstructor = this.objeto?.constructor as FormCampoConstrutor;
    if (objetoConstructor) {
      this.formFields = objetoConstructor.formFields ?? [];

      console.log('Campos definidos no formFields:', this.formFields.map(f => f.key));
      
      // Inicializa o formulário com controles baseados nos metadados
      this.form = this.fb.group(
        this.formFields.reduce((acc, field) => {
          acc[field.key] = ['', field.required ? [Validators.required] : []];
          return acc;
        }, {} as { [key: string]: any[] })
      );
    }

    // Obter dados do `state` da navegação (se aplicável)
    const state = window.history.state;
    if (state && state.objeto) {
      try {
        this.dados = state.objeto;

        // Verificar se `this.dados` é uma string ou array inesperada
        if (typeof this.dados === 'string') {
          this.dados = JSON.parse(this.dados);
        }

        if (Array.isArray(this.dados)) {
          this.dados = this.dados[0]; // Suponha que o primeiro item do array seja o objeto correto
        }

        console.log('Dados corrigidos para o formato correto:', this.dados);

        // Define `isEditMode` com base na presença de `id`
        this.isEditMode = !!this.dados.id; // Supondo que a presença de um `id` indica modo de edição
      } catch (error) {
        console.error('Erro ao analisar dados do objeto:', error);
      }
    }
  }

  ngAfterViewInit() {
    // Preenche o formulário após a view ter sido renderizada
    setTimeout(() => {
      this.preencherFormulario();
    });
  }

  preencherFormulario() {
    if (this.dados && this.form) {
      console.log('Preenchendo o formulário com:', this.dados);

      const mappedData: { [key: string]: any } = {};

      this.formFields.forEach(field => {
        // Certifica-se de que a chave de `field` existe em `this.dados`
        if (this.dados.hasOwnProperty(field.key)) {
          mappedData[field.key] = this.dados[field.key];
        } else {
          console.warn(`Campo não encontrado em dados: ${field.key}`);
        }
      });

      console.log('Dados mapeados para patchValue:', mappedData);

      this.form.patchValue(mappedData);

      console.log('Valor do formulário após patchValue:', this.form.value);

      this.cd.detectChanges();
    }
  }

  // Função para retornar à última rota
  voltar() {
    this.location.back();
  }

  onSubmit() {
    if (this.form.valid) {
      this.salvarCallback(this.form.value);
    } else {
      this.toastService.exibirMensagemErro('Erro', 'Formulário inválido');
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
