import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DeviceService } from '../../services/device.service';
import { Device, User } from '../../models/device.model';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './device-form.html',
  styleUrls: ['./device-form.css'],
})
export class DeviceFormComponent implements OnInit {
  deviceForm: FormGroup;
  isEditMode = false;
  deviceId?: number;
  users: User[] = [];
  existingDevices: Device[] = [];

  constructor(
    private fb: FormBuilder,
    private deviceService: DeviceService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.deviceForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      manufacturer: ['', Validators.required],
      type: ['', Validators.required],
      ramAmount: ['', Validators.required],
      os: [''],
      osVersion: [''],
      processor: [''],
      description: [''], 
      userId: [null],
    });
  }

  ngOnInit(): void {
    this.deviceService.getUsers().subscribe((data) => (this.users = data));
    this.deviceService.getDevices().subscribe((data) => (this.existingDevices = data));

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEditMode = true;
      this.deviceId = +idParam;
      this.deviceService.getDevice(this.deviceId).subscribe((device) => {
        this.deviceForm.patchValue(device);
      });
    }
  }

  onSubmit(): void {
    if (this.deviceForm.invalid) {
      alert('Please fill in all required fields.');
      return;
    }

    const deviceData = this.deviceForm.value;

    if (!this.isEditMode) {
      const exists = this.existingDevices.some(
        (d) => d.name.toLowerCase() === deviceData.name.toLowerCase(),
      );
      if (exists) {
        alert('A device with this name already exists!');
        return;
      }
    }

    if (this.isEditMode && this.deviceId) {
      this.deviceService.updateDevice(this.deviceId, deviceData).subscribe(() => {
        this.router.navigate(['/']);
      });
    } else {
      this.deviceService.createDevice(deviceData).subscribe(() => {
        this.router.navigate(['/']);
      });
    }
  }
}
