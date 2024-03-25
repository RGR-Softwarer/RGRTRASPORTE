import { environment } from '../../../environments/environment';

export const ClienteUrls = {
    Login: `${environment.apiBaseUrl}/Auth/login`,
} as const;