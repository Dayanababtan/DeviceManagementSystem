import { Component, signal } from '@angular/core';
import { RouterLink, RouterOutlet, RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  title = 'Device Management System';
}
