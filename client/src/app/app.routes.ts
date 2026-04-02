import { Routes } from '@angular/router';
import { DeviceList } from './components/device-list/device-list';
import { DeviceDetails } from './components/device-details/device-details';
import { DeviceFormComponent} from './components/device-form/device-form';
import { RegisterComponent } from './components/register/registe';
import { LoginComponent } from './components/login/login';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  { 
    path: '', 
    component: DeviceList, 
    canActivate: [authGuard] 
  },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  
  { path: 'devices', component: DeviceList, canActivate: [authGuard] },
  { path: 'details/:id', component: DeviceDetails, canActivate: [authGuard] },
  { path: 'add', component: DeviceFormComponent, canActivate: [authGuard] },
  { path: 'edit/:id', component: DeviceFormComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];