import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppContextService } from './services/context/app.context';
import { ConfirmComponent } from './shared/components/confirm/confirm.component';
import { NotificationComponent } from './shared/components/notification/notification.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [RouterOutlet, ConfirmComponent, NotificationComponent],
  standalone: true
})

export class AppComponent {
  title = 'RGRTRASPORTE';

}
