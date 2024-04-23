import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PacoteComponent } from './pacote.component';

describe('PacoteComponent', () => {
  let component: PacoteComponent;
  let fixture: ComponentFixture<PacoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PacoteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PacoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
