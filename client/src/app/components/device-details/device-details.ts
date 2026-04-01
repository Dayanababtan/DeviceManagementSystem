import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device, User } from '../../models/device.model';
import { Observable, forkJoin } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './device-details.html',
  styleUrl: './device-details.css',
})
export class DeviceDetails implements OnInit {
  details$!: Observable<{ device: Device, userName: string }>;

  constructor(
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {}

  ngOnInit(): void {
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
            userName: owner ? owner.name : 'Unassigned'
          };
        })
      );
    }
  }
}