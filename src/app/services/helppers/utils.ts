import { MessageService } from 'primeng/api';

export class Util {
  static showSuccessMessage(messageService: MessageService, message: string): void {
    messageService.add({ severity: 'success', summary: 'Success', detail: message });
  }
}
