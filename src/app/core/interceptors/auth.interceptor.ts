import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppContextService } from '../../services/context/app.context';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  
  constructor(private appContextService: AppContextService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const headers: { [key: string]: string } = {};
    
    // Adicionar token JWT se disponível
    const appContext = this.appContextService.obterUsuarioLogado();
    if (appContext?.token) {
      headers['Authorization'] = appContext.token.startsWith('Bearer ') 
        ? appContext.token 
        : `Bearer ${appContext.token}`;
    }
    
    // Adicionar header X-Tenant-Id (multitenancy)
    // Para desenvolvimento local, usa localhost:5001
    // Em produção, pode ser configurado dinamicamente
    let tenantId = 'localhost:5001'; // Valor padrão
    
    try {
      // Se a URL é absoluta (começa com http:// ou https://)
      if (req.url.startsWith('http://') || req.url.startsWith('https://')) {
        const url = new URL(req.url);
        tenantId = url.hostname === 'localhost' && url.port === '5001' 
          ? 'localhost:5001' 
          : url.host || 'localhost:5001';
      } else {
        // URL relativa - usar o host atual
        tenantId = window.location.hostname === 'localhost' && window.location.port === '5001'
          ? 'localhost:5001'
          : window.location.host || 'localhost:5001';
      }
    } catch {
      // Fallback se não conseguir parsear a URL
      tenantId = 'localhost:5001';
    }
    
    headers['X-Tenant-Id'] = tenantId;
    
    // Clonar a requisição com os headers adicionais
    if (Object.keys(headers).length > 0) {
      const authReq = req.clone({
        setHeaders: headers
      });
      return next.handle(authReq);
    }
    
    return next.handle(req);
  }
}
