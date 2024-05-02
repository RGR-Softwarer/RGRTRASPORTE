import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PacoteFormularioComponent } from './pacote-formulario.component';

describe('PacoteFormularioComponent', () => {
  let component: PacoteFormularioComponent;
  let fixture: ComponentFixture<PacoteFormularioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PacoteFormularioComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PacoteFormularioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
