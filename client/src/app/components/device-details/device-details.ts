import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device, User } from '../../models/device.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './device-details.html',
  styleUrl: './device-details.css',
})
export class DeviceDetails implements OnInit {
  device?: Device;
  userName: string = 'Loading...';

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      forkJoin({
        device: this.deviceService.getDevice(id),
        users: this.deviceService.getUsers()
      }).subscribe({
        next: (data) => {
          this.device = data.device;
          const owner = data.users.find(u => u.userId === this.device?.userId);
          this.userName = owner ? owner.name : 'Unassigned';
        },
        error: (err) => console.error('Could not load device details', err)
      });
    }
  }
}