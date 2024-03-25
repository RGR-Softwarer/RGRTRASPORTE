import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormCampoConstrutor } from '../../services/decorator/formulario-decorator';
import { ToastService } from '../../services/utils/notificacao/toast.service';

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styleUrl: './formulario.component.scss'
})
export class FormularioComponent implements OnInit {
  @Input() salvarCallback: (data: any) => void = () => { };
  @Input() objeto: any;

  form: FormGroup = new FormGroup({});
  formFields: any[] = [];

  constructor(private fb: FormBuilder, private toastService: ToastService) { }

  ngOnInit() {
    const objetoConstructor = this.objeto.constructor as FormCampoConstrutor;
    this.formFields = objetoConstructor.formFields ?? [];
    this.form = this.fb.group({});

    this.formFields.forEach(field => {
      const validators = field.required ? [Validators.required] : [];
      this.form.addControl(field.key, this.fb.control('', validators));
    });
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