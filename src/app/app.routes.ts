import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { CanchasComponent } from './components/canchas/canchas.component';
import { HorariosComponent } from './components/horarios/horarios.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'canchas', component: CanchasComponent },
  { path: 'horarios/:id', component: HorariosComponent },
  {
    path: 'historial',
    loadComponent: () =>
      import('./components/historial/historial.component').then(m => m.HistorialComponent)
  }
];
