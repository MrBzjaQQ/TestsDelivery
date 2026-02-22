import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of, map } from 'rxjs';
import { environment } from '../../../environments/environment';

export type UserRole = 'Student' | 'Teacher' | 'Admin';

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  emailVerified: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: User;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: 'Student' | 'Teacher';
}

export interface RegisterResponse {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  emailVerified: boolean;
  createdAt: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface RefreshTokenResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface ApiError {
  type: string;
  title: string;
  status: number;
  detail: string;
  instance: string;
  traceId: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly router = inject(Router);
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  private readonly _user = signal<User | null>(null);
  private readonly _isAuthenticated = signal<boolean>(false);
  private readonly _isLoading = signal<boolean>(false);

  readonly user = this._user.asReadonly();
  readonly isAuthenticated = this._isAuthenticated.asReadonly();
  readonly isLoading = this._isLoading.asReadonly();

  readonly fullName = computed(() => {
    const user = this._user();
    return user ? `${user.firstName} ${user.lastName}` : '';
  });

  readonly userRole = computed(() => {
    return this._user()?.role ?? null;
  });

  readonly isStudent = computed(() => this._user()?.role === 'Student');
  readonly isTeacher = computed(() => this._user()?.role === 'Teacher' || this._user()?.role === 'Admin');
  readonly isAdmin = computed(() => this._user()?.role === 'Admin');

  constructor() {
    this.loadFromStorage();
  }

  private loadFromStorage(): void {
    const token = localStorage.getItem('access_token');
    const refreshToken = localStorage.getItem('refresh_token');
    const userData = localStorage.getItem('user_data');

    if (token && userData) {
      try {
        const user = JSON.parse(userData) as User;
        this._user.set(user);
        this._isAuthenticated.set(true);
      } catch {
        this.clearAuth();
      }
    }
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    this._isLoading.set(true);
    return this.http.post<{ isError: boolean; data: LoginResponse }>(`${this.baseUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          if (!response.isError && response.data) {
            this.setTokens(response.data.accessToken, response.data.refreshToken);
            this._user.set(response.data.user);
            this._isAuthenticated.set(true);
          }
          this._isLoading.set(false);
        }),
        catchError(error => {
          this._isLoading.set(false);
          throw error;
        }),
        map(response => response.data)
      );
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    this._isLoading.set(true);
    return this.http.post<{ isError: boolean; data: RegisterResponse }>(`${this.baseUrl}/auth/register`, request)
      .pipe(
        tap(response => {
          this._isLoading.set(false);
        }),
        catchError(error => {
          this._isLoading.set(false);
          throw error;
        }),
        map(response => response.data)
      );
  }

  logout(): void {
    const refreshToken = localStorage.getItem('refresh_token');
    if (refreshToken) {
      this.http.post(`${this.baseUrl}/auth/logout`, {}).subscribe({
        complete: () => this.handleLogout()
      });
    } else {
      this.handleLogout();
    }
  }

  forgotPassword(email: string): Observable<{ isError: boolean; message: string }> {
    return this.http.post<{ isError: boolean; message: string }>(`${this.baseUrl}/auth/forgot-password`, { email });
  }

  resetPassword(token: string, newPassword: string): Observable<{ isError: boolean; message: string }> {
    return this.http.post<{ isError: boolean; message: string }>(`${this.baseUrl}/auth/reset-password`, { token, newPassword });
  }

  private handleLogout(): void {
    this.clearAuth();
    this.router.navigate(['/login']);
  }

  private clearAuth(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('user_data');
    this._user.set(null);
    this._isAuthenticated.set(false);
  }

  private setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem('access_token', accessToken);
    localStorage.setItem('refresh_token', refreshToken);
    localStorage.setItem('user_data', JSON.stringify(this._user()));
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token');
  }

  refreshToken(): Observable<RefreshTokenResponse | null> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return of(null);
    }

    return this.http.post<{ isError: boolean; data: RefreshTokenResponse }>(
      `${this.baseUrl}/auth/refresh`,
      { refreshToken }
    ).pipe(
      tap(response => {
        if (!response.isError && response.data) {
          this.setTokens(response.data.accessToken, response.data.refreshToken);
        }
      }),
      catchError(() => {
        this.logout();
        return of(null);
      }),
    ) as Observable<RefreshTokenResponse | null>;
  }

  hasRole(roles: UserRole | UserRole[]): boolean {
    const user = this._user();
    if (!user) return false;

    const roleArray = Array.isArray(roles) ? roles : [roles];
    return roleArray.includes(user.role);
  }

  // Demo login for development
  demoLogin(role: UserRole = 'Student'): void {
    const demoUsers: Record<UserRole, User> = {
      Student: {
        id: '00000000-0000-0000-0000-000000000001',
        email: 'student@test.com',
        firstName: 'Иван',
        lastName: 'Петров',
        role: 'Student',
        emailVerified: true
      },
      Teacher: {
        id: '00000000-0000-0000-0000-000000000002',
        email: 'teacher@test.com',
        firstName: 'Пётр',
        lastName: 'Иванов',
        role: 'Teacher',
        emailVerified: true
      },
      Admin: {
        id: '00000000-0000-0000-0000-000000000003',
        email: 'admin@test.com',
        firstName: 'Админ',
        lastName: 'Админов',
        role: 'Admin',
        emailVerified: true
      }
    };

    const token = 'demo-token-' + Date.now();
    this.loginWithToken(token, demoUsers[role]);
  }

  loginWithToken(token: string, user: User): void {
    localStorage.setItem('access_token', token);
    localStorage.setItem('user_data', JSON.stringify(user));
    this._user.set(user);
    this._isAuthenticated.set(true);
  }
}
