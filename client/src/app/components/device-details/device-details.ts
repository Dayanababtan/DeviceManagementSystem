import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { AuthService } from '../../services/auth.service';
import { Device, User } from '../../models/device.model';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './device-details.html',
  styleUrl: './device-details.css',
})
export class DeviceDetails implements OnInit {
  details$!: Observable<{ device: Device; owner: User | null }>;
  currentUserId: number | null = null;
  // We keep this variable for logic, but we rename the template in HTML to avoid the crash
  isLoadingData = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.loadData();
  }

  loadData(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.isLoadingData = true;

    if (id) {
      this.details$ = forkJoin({
        device: this.deviceService.getDevice(id),
        users: this.deviceService.getUsers(),
      }).pipe(
        map((data) => {
          this.isLoadingData = false;
          const owner = data.users.find((u) => u.userId === data.device.userId) || null;
          return { device: data.device, owner: owner };
        }),
        catchError((err) => {
          this.isLoadingData = false;
          console.error('Error loading details', err);
          this.router.navigate(['/']);
          return of();
        }),
      );
    }
  }

  assignToMe(device: Device): void {
    if (!this.currentUserId) return;
    const updatedDevice = { ...device, userId: this.currentUserId };

    this.deviceService.updateDevice(device.deviceId, updatedDevice).subscribe({
      next: () => {
        alert('Device assigned to you!');
        this.loadData();
      },
    });
  }

  unassignMe(device: Device): void {
    const updatedDevice = { ...device, userId: 0 }; // Or null, depending on your backend
    this.deviceService.updateDevice(device.deviceId, updatedDevice).subscribe({
      next: () => {
        alert('Device released.');
        this.loadData();
      },
    });
  }
}
