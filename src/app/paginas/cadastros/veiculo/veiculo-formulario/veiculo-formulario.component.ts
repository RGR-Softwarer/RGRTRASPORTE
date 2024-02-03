import { Component } from '@angular/core';
import { Veiculo } from '../../../../dominio/entidade/veiculo';

@Component({
  selector: 'app-veiculo-formulario',  
  templateUrl: './veiculo-formulario.component.html',
  styleUrl: './veiculo-formulario.component.scss'
})
export class VeiculoFormularioComponent  {
  
  veiculo = new Veiculo();

  Salvar(data: any) {
    console.log('Dados salvos pelo pai:', data);
  }

}