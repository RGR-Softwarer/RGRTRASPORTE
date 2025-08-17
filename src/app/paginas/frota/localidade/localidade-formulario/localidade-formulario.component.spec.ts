import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute } from '@angular/router';
import { LocalidadeFormularioComponent } from './localidade-formulario.component';
import { LocalidadeFacade } from '../services/localidade.facade';
import { NotificationService } from '../../../../shared/services/notification.service';
import { of } from 'rxjs';

describe('LocalidadeFormularioComponent', () => {
  let component: LocalidadeFormularioComponent;
  let fixture: ComponentFixture<LocalidadeFormularioComponent>;
  let mockFacade: jasmine.SpyObj<LocalidadeFacade>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const facadeSpy = jasmine.createSpyObj('LocalidadeFacade', [
      'buscarLocalidadePorId',
      'salvarLocalidade'
    ]);

    const notificationSpy = jasmine.createSpyObj('NotificationService', [
      'success',
      'error',
      'warning'
    ]);

    await TestBed.configureTestingModule({
      imports: [LocalidadeFormularioComponent, ReactiveFormsModule, RouterTestingModule],
      providers: [
        { provide: LocalidadeFacade, useValue: facadeSpy },
        { provide: NotificationService, useValue: notificationSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ id: '1' })
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LocalidadeFormularioComponent);
    component = fixture.componentInstance;
    mockFacade = TestBed.inject(LocalidadeFacade) as jasmine.SpyObj<LocalidadeFacade>;
    mockNotificationService = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with default values', () => {
    expect(component.localidadeForm).toBeDefined();
    expect(component.localidadeForm.get('ativo')?.value).toBe(true);
  });

  it('should validate required fields', () => {
    const form = component.localidadeForm;
    
    expect(form.get('nome')?.hasError('required')).toBeTruthy();
    expect(form.get('estado')?.hasError('required')).toBeTruthy();
    expect(form.get('cidade')?.hasError('required')).toBeTruthy();
    expect(form.get('cep')?.hasError('required')).toBeTruthy();
    expect(form.get('bairro')?.hasError('required')).toBeTruthy();
    expect(form.get('logradouro')?.hasError('required')).toBeTruthy();
    expect(form.get('latitude')?.hasError('required')).toBeTruthy();
    expect(form.get('longitude')?.hasError('required')).toBeTruthy();
  });

  it('should validate latitude range', () => {
    const latitudeControl = component.localidadeForm.get('latitude');
    
    latitudeControl?.setValue(-91);
    expect(latitudeControl?.hasError('min')).toBeTruthy();
    
    latitudeControl?.setValue(91);
    expect(latitudeControl?.hasError('max')).toBeTruthy();
    
    latitudeControl?.setValue(-23.5505);
    expect(latitudeControl?.hasError('min')).toBeFalsy();
    expect(latitudeControl?.hasError('max')).toBeFalsy();
  });

  it('should validate longitude range', () => {
    const longitudeControl = component.localidadeForm.get('longitude');
    
    longitudeControl?.setValue(-181);
    expect(longitudeControl?.hasError('min')).toBeTruthy();
    
    longitudeControl?.setValue(181);
    expect(longitudeControl?.hasError('max')).toBeTruthy();
    
    longitudeControl?.setValue(-46.6333);
    expect(longitudeControl?.hasError('min')).toBeFalsy();
    expect(longitudeControl?.hasError('max')).toBeFalsy();
  });
});