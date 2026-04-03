import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { AuthService } from '../../services/auth.service'; // Added
import { Device } from '../../models/device.model';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './device-details.html',
  styleUrl: './device-details.css',
})
export class DeviceDetails implements OnInit {
  details$!: Observable<{ device: Device; userName: string }>;
  currentUserId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private deviceService: DeviceService,
    private authService: AuthService 
  ) {}

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.loadData();
  }

  loadData(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      this.details$ = forkJoin({
        device: this.deviceService.getDevice(id),
        users: this.deviceService.getUsers()
      }).pipe(
        map(data => {
          const owner = data.users.find(u => u.userId === data.device.userId);
          return {
            device: data.device,
            userName: owner ? owner.name : 'Not Assigned Yet'
          };
        }),
        catchError(err => {
          console.error('Error loading details', err);
          this.router.navigate(['/']); 
          return of();
        })
      );
    }
  }

  assignToMe(device: Device): void {
    if (!this.currentUserId) {
      this.router.navigate(['/login']);
      return;
    }

    const updatedDevice = { ...device, userId: this.currentUserId };
    
    this.deviceService.updateDevice(device.deviceId, updatedDevice).subscribe({
      next: () => {
        alert('Device successfully assigned to you!');
        this.loadData(); // Refresh the UI
      },
      error: (err) => console.error('Assignment failed', err)
    });
  }

  unassignMe(device: Device): void {
    const updatedDevice = { ...device, userId: null };

    this.deviceService.updateDevice(device.deviceId, updatedDevice).subscribe({
      next: () => {
        alert('Device released.');
        this.loadData(); 
      },
      error: (err) => console.error('Unassignment failed', err)
    });
  }
}