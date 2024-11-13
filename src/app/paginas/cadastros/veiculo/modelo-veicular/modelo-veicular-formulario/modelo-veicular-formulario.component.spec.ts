import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModeloVeicularFormularioComponent } from './modelo-veicular-formulario.component';

describe('ModeloVeicularFormularioComponent', () => {
  let component: ModeloVeicularFormularioComponent;
  let fixture: ComponentFixture<ModeloVeicularFormularioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModeloVeicularFormularioComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModeloVeicularFormularioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
