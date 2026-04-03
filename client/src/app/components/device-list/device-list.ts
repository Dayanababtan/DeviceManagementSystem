import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { AuthService } from '../../services/auth.service'; // 1. Import AuthService
import {
  forkJoin,
  BehaviorSubject,
  take,
  Subject,
  Observable,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  startWith,
  map,
} from 'rxjs';

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

  currentUserId: number | null = null;

  private searchTerms = new Subject<string>();

  constructor(
    private deviceService: DeviceService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();

    this.devicesWithUsers$ = this.searchTerms.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) =>
        forkJoin({
          devices: this.deviceService.searchDevices(term),
          users: this.deviceService.getUsers(),
        }),
      ),
      map((result) => {
        return result.devices.map((device) => ({
          ...device,
          ownerName:
            result.users.find((u) => u.userId == device.userId)?.name || 'Not Assigned Yet',
        }));
      }),
    );
  }

  search(term: string): void {
    this.searchTerms.next(term);
  }
  deleteDevice(id: number): void {
    const deviceToDelete = this.devicesSubject.value.find((d) => d.deviceId === id);
    if (
      deviceToDelete &&
      deviceToDelete.userId !== null &&
      deviceToDelete.userId !== this.currentUserId
    ) {
      alert('You do not have permission to delete this device.');
      return;
    }

    if (confirm('Are you sure you want to delete this device?')) {
      this.deviceService.deleteDevice(id).subscribe(() => {
        const currentList = this.devicesSubject.value;
        const updatedList = currentList.filter((dev) => dev.deviceId !== id);
        this.devicesSubject.next(updatedList);
      });
    }
  }
}
