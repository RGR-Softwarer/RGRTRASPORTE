import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LocalidadeComponent } from './localidade.component';
import { LocalidadeFacade } from './services/localidade.facade';
import { NotificationService } from '../../../shared/services/notification.service';
import { ConfigService } from '../../../services/config/config.service';
import { of } from 'rxjs';

describe('LocalidadeComponent', () => {
  let component: LocalidadeComponent;
  let fixture: ComponentFixture<LocalidadeComponent>;
  let mockFacade: jasmine.SpyObj<LocalidadeFacade>;
  let mockNotificationService: jasmine.SpyObj<NotificationService>;
  let mockConfigService: jasmine.SpyObj<ConfigService>;

  beforeEach(async () => {
    const facadeSpy = jasmine.createSpyObj('LocalidadeFacade', [
      'carregarLocalidades',
      'deletarLocalidade'
    ], {
      localidades$: of([]),
      isLoading$: of(false)
    });

    const notificationSpy = jasmine.createSpyObj('NotificationService', [
      'success',
      'error',
      'warning'
    ]);

    const configSpy = jasmine.createSpyObj('ConfigService', ['getApiBaseUrl']);
    configSpy.getApiBaseUrl.and.returnValue('http://localhost:4000/api');

    await TestBed.configureTestingModule({
      imports: [LocalidadeComponent, RouterTestingModule],
      providers: [
        { provide: LocalidadeFacade, useValue: facadeSpy },
        { provide: NotificationService, useValue: notificationSpy },
        { provide: ConfigService, useValue: configSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LocalidadeComponent);
    component = fixture.componentInstance;
    mockFacade = TestBed.inject(LocalidadeFacade) as jasmine.SpyObj<LocalidadeFacade>;
    mockNotificationService = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
    mockConfigService = TestBed.inject(ConfigService) as jasmine.SpyObj<ConfigService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with correct properties', () => {
    fixture.detectChanges();
    expect(component.buscarTodosUrl).toBe('http://localhost:4000/api/Localidade');
    expect(component.adicionarUrl).toBe('/frota/localidade/adicionar');
    expect(component.identificador).toBe('localidade-grid');
  });

  it('should call facade.deletarLocalidade when deletar is called with confirmation', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    
    component.deletar(1);
    
    expect(mockFacade.deletarLocalidade).toHaveBeenCalledWith(1);
  });

  it('should not call facade.deletarLocalidade when deletar is called without confirmation', () => {
    spyOn(window, 'confirm').and.returnValue(false);
    
    component.deletar(1);
    
    expect(mockFacade.deletarLocalidade).not.toHaveBeenCalled();
  });

  it('should call facade.carregarLocalidades and show success message when recarregarDados is called', () => {
    component.recarregarDados();
    
    expect(mockFacade.carregarLocalidades).toHaveBeenCalled();
    expect(mockNotificationService.success).toHaveBeenCalledWith('Sucesso', 'Dados recarregados com sucesso!');
  });
});