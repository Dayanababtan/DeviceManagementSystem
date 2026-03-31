import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router'; 
import { DeviceService } from '../../services/device.service';
import { Device, User } from '../../models/device.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [CommonModule, RouterModule], 
  templateUrl: './device-list.html',
  styleUrl: './device-list.css',
})
export class DeviceList implements OnInit {
  devicesWithUsers: any[] = [];

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
  forkJoin({
    devices: this.deviceService.getDevices(),
    users: this.deviceService.getUsers()
  }).subscribe({
    next: (result) => {
      this.devicesWithUsers = result.devices.map(device => {
        // MATCHING LOGIC: 
        // Compare device.userId to user.userId (both lowercase)
        const owner = result.users.find(u => u.userId === device.userId);
        
        return {
          ...device,
          // We create 'ownerName' to use in the HTML
          ownerName: owner ? owner.name : 'No Owner' 
        };
      });
    },
    error: (err) => console.error('Error loading data', err)
  });
}

  deleteDevice(id: number): void {
    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe(() => {
        this.loadDevices();
      });
    }
  }
}