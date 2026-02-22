import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

export const guestGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    return true;
  }

  const user = authService.user();
  if (user) {
    switch (user.role) {
      case 'Admin':
        router.navigate(['/admin/dashboard']);
        break;
      case 'Teacher':
        router.navigate(['/teacher/dashboard']);
        break;
      default:
        router.navigate(['/dashboard']);
    }
  }

  return false;
};
