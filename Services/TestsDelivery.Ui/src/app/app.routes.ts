import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { guestGuard } from './core/guards/guest.guard';
import { LoginPageComponent } from './pages/login/login.page';
import { RegisterPageComponent } from './pages/register/register.page';
import { ForgotPasswordPageComponent } from './pages/forgot-password/forgot-password.page';
import { ResetPasswordPageComponent } from './pages/reset-password/reset-password.page';
import { DashboardPageComponent } from './pages/dashboard/dashboard.page';
import { TestPageComponent } from './pages/test/test.page';
import { ResultsPageComponent } from './pages/results/results.page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    canActivate: [guestGuard],
    component: LoginPageComponent
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    component: RegisterPageComponent
  },
  {
    path: 'forgot-password',
    canActivate: [guestGuard],
    component: ForgotPasswordPageComponent
  },
  {
    path: 'reset-password',
    canActivate: [guestGuard],
    component: ResetPasswordPageComponent
  },
  {
    path: 'dashboard',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Student'] },
    component: DashboardPageComponent
  },
  {
    path: 'test/:id',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Student'] },
    component: TestPageComponent
  },
  {
    path: 'results/:id',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Student'] },
    component: ResultsPageComponent
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
