// client/src/app/models/device.model.ts
export interface User {
  userId: number;
  name: string;
  role: string;
  location: string;
}

export interface Device {
  deviceId: number;
  name: string;
  manufacturer: string;
  type: string;
  ramAmount: string;
  userId: number; 
}