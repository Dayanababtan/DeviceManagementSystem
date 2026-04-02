import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router'; 
import { DeviceService } from '../../services/device.service';
import { AuthService } from '../../services/auth.service'; // 1. Import AuthService
import { forkJoin, BehaviorSubject, take } from 'rxjs';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, RouterModule], 
  templateUrl: './device-list.html',
  styleUrl: './device-list.css',
})
export class DeviceList implements OnInit {
  private devicesSubject = new BehaviorSubject<any[]>([]);
  devicesWithUsers$ = this.devicesSubject.asObservable();

  // 2. Create a property for the template to use
  currentUserId: number | null = null;

  // 3. Inject AuthService in the constructor
  constructor(
    private deviceService: DeviceService,
    private authService: AuthService 
  ) {}

  ngOnInit(): void {
    // 4. Set the current user ID immediately
    this.currentUserId = this.authService.getUserId();
    this.loadDevices();
  }

  loadDevices(): void {
    forkJoin({
      devices: this.deviceService.getDevices(),
      users: this.deviceService.getUsers()
    }).pipe(take(1)).subscribe(result => {
      const mappedData = result.devices.map(device => ({
        ...device,
        // We ensure userId is preserved here so dev.userId works in HTML
        ownerName: result.users.find(u => u.userId == device.userId)?.name || 'Not Assigned Yet'
      }));
      
      this.devicesSubject.next(mappedData);
    });
  }

  deleteDevice(id: number): void {
    // Safety Check: Verify ownership/permission again before calling delete
    const deviceToDelete = this.devicesSubject.value.find(d => d.deviceId === id);
    if (deviceToDelete && deviceToDelete.userId !== null && deviceToDelete.userId !== this.currentUserId) {
      alert("You do not have permission to delete this device.");
      return;
    }

    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe(() => {
        const currentList = this.devicesSubject.value;
        const updatedList = currentList.filter(dev => dev.deviceId !== id);
        this.devicesSubject.next(updatedList);
      });
    }
  }
}