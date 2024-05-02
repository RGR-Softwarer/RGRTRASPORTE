import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViagemFormularioComponent } from './viagem-formulario.component';

describe('ViagemFormularioComponent', () => {
  let component: ViagemFormularioComponent;
  let fixture: ComponentFixture<ViagemFormularioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViagemFormularioComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViagemFormularioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
