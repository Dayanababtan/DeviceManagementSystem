import { Routes } from '@angular/router';
import { DeviceList } from './components/device-list/device-list';
import { DeviceDetails } from './components/device-details/device-details';
import { DeviceFormComponent} from './components/device-form/device-form';

export const routes: Routes = [
  { path: '', component: DeviceList },
  { path: 'devices', component: DeviceList },
  { path: 'details/:id', component: DeviceDetails },
  { path: 'add', component: DeviceFormComponent },
  { path: 'edit/:id', component: DeviceFormComponent },
  { path: '**', redirectTo: '' }
];