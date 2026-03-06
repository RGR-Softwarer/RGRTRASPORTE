import { environment } from '../../../environments/environment';

export const ClienteUrls = {
    Login: `${environment.apiTrasportador}auth/login`,
} as const;