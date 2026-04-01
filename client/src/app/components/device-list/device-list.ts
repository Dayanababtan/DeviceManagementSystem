import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router'; 
import { DeviceService } from '../../services/device.service';
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

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    forkJoin({
      devices: this.deviceService.getDevices(),
      users: this.deviceService.getUsers()
    }).pipe(take(1)).subscribe(result => {
      const mappedData = result.devices.map(device => ({
        ...device,
        ownerName: result.users.find(u => u.userId == device.userId)?.name || 'Not Assigned Yet'
      }));
      
      this.devicesSubject.next(mappedData);
    });
  }

  deleteDevice(id: number): void {
    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe(() => {
        const currentList = this.devicesSubject.value;
        const updatedList = currentList.filter(dev => dev.deviceId !== id);
        
        this.devicesSubject.next(updatedList);
      });
    }
  }
}