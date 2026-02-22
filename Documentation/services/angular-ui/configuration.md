# Angular 21 UI - Конфигурация

## Конфигурация приложения

### Файлы конфигурации

```
src/
├── environments/
│   ├── environment.ts           # Development
│   └── environment.prod.ts       # Production
```

### Environment

**environment.ts (Development):**
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:8080/api/v1',
  fileServiceUrl: 'http://localhost:8085',
  identityUrl: 'http://localhost:8081',
  version: '1.0.0-dev',
  features: {
    enableDebugTools: true,
    enableAnalytics: false,
    enableNotifications: true
  },
  timeouts: {
    apiRequest: 30000,
    tokenRefresh: 60000
  },
  pagination: {
    defaultPageSize: 10,
    pageSizeOptions: [10, 25, 50, 100]
  }
};
```

**environment.prod.ts (Production):**
```typescript
export const environment = {
  production: true,
  apiUrl: '/api/v1',
  fileServiceUrl: '/files',
  identityUrl: '/identity',
  version: '1.0.0',
  features: {
    enableDebugTools: false,
    enableAnalytics: true,
    enableNotifications: true
  },
  timeouts: {
    apiRequest: 30000,
    tokenRefresh: 60000
  },
  pagination: {
    defaultPageSize: 10,
    pageSizeOptions: [10, 25, 50, 100]
  }
};
```

---

## Конфигурация Angular

### angular.json

```json
{
  "$schema": "./node_modules/@angular/cli/lib/config/schema.json",
  "version": 1,
  "newProjectRoot": "projects",
  "projects": {
    "testsdelivery-web": {
      "projectType": "application",
      "schematics": {
        "@schematics/angular:component": {
          "style": "scss",
          "standalone": true,
          "changeDetection": "OnPush"
        },
        "@schematics/angular:directive": {
          "standalone": true
        },
        "@schematics/angular:pipe": {
          "standalone": true
        }
      },
      "root": "",
      "sourceRoot": "src",
      "prefix": "app",
      "architect": {
        "build": {
          "builder": "@angular-devkit/build-angular:application",
          "options": {
            "outputPath": "dist/testsdelivery-web",
            "index": "src/index.html",
            "browser": "src/main.ts",
            "polyfills": ["zone.js"],
            "tsConfig": "tsconfig.app.json",
            "inlineStyleLanguage": "scss",
            "assets": [
              "src/favicon.ico",
              "src/assets"
            ],
            "styles": [
              "src/styles.scss"
            ],
            "scripts": []
          },
          "configurations": {
            "production": {
              "budgets": [
                {
                  "type": "initial",
                  "maximumWarning": "500kb",
                  "maximumError": "1mb"
                },
                {
                  "type": "anyComponentStyle",
                  "maximumWarning": "2kb",
                  "maximumError": "4kb"
                }
              ],
              "outputHashing": "all",
              "optimization": true
            },
            "development": {
              "optimization": false,
              "extractLicenses": false,
              "sourceMap": true
            }
          },
          "defaultConfiguration": "production"
        },
        "serve": {
          "builder": "@angular-devkit/build-angular:dev-server",
          "configurations": {
            "production": {
              "buildTarget": "testsdelivery-web:build:production"
            },
            "development": {
              "buildTarget": "testsdelivery-web:build:development"
            }
          },
          "defaultConfiguration": "development"
        },
        "test": {
          "builder": "@angular-devkit/build-angular:karma",
          "options": {
            "polyfills": ["zone.js", "zone.js/testing"],
            "tsConfig": "tsconfig.spec.json",
            "inlineStyleLanguage": "scss",
            "assets": [
              "src/favicon.ico",
              "src/assets"
            ],
            "styles": [
              "src/styles.scss"
            ],
            "scripts": []
          }
        }
      }
    }
  }
}
```

---

### tsconfig.json

```json
{
  "compileOnSave": false,
  "compilerOptions": {
    "outDir": "./dist/out-tsc",
    "forceConsistentCasingInFileNames": true,
    "strict": true,
    "noImplicitOverride": true,
    "noPropertyAccessFromIndexSignature": true,
    "noImplicitReturns": true,
    "noFallthroughCasesInSwitch": true,
    "skipLibCheck": true,
    "esModuleInterop": true,
    "sourceMap": true,
    "declaration": false,
    "experimentalDecorators": true,
    "moduleResolution": "bundler",
    "importHelpers": true,
    "target": "ES2022",
    "module": "ES2022",
    "useDefineForClassFields": false,
    "lib": ["ES2022", "dom"],
    "baseUrl": "./",
    "paths": {
      "@core/*": ["src/app/core/*"],
      "@features/*": ["src/app/features/*"],
      "@shared/*": ["src/app/shared/*"],
      "@layout/*": ["src/app/layout/*"],
      "@env/*": ["src/environments/*"]
    }
  },
  "angularCompilerOptions": {
    "enableI18nLegacyMessageIdFormat": false,
    "strictInjectionParameters": true,
    "strictInputAccessModifiers": true,
    "strictTemplates": true
  }
}
```

---

## Конфигурация стилей

### _variables.scss

```scss
// Цветовая палитра
$primary: #2563eb;
$primary-dark: #1d4ed8;
$primary-light: #3b82f6;

$secondary: #64748b;
$secondary-dark: #475569;
$secondary-light: #94a3b8;

$success: #22c55e;
$danger: #ef4444;
$warning: #f59e0b;
$info: #3b82f6;

// Серые оттенки
$gray-50: #f8fafc;
$gray-100: #f1f5f9;
$gray-200: #e2e8f0;
$gray-300: #cbd5e1;
$gray-400: #94a3b8;
$gray-500: #64748b;
$gray-600: #475569;
$gray-700: #334155;
$gray-800: #1e293b;
$gray-900: #0f172a;

// Типографика
$font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
$font-size-xs: 0.75rem;    // 12px
$font-size-sm: 0.875rem;   // 14px
$font-size-base: 1rem;     // 16px
$font-size-lg: 1.125rem;   // 18px
$font-size-xl: 1.25rem;    // 20px
$font-size-2xl: 1.5rem;    // 24px
$font-size-3xl: 1.875rem;  // 30px
$font-size-4xl: 2.25rem;   // 36px

$font-weight-normal: 400;
$font-weight-medium: 500;
$font-weight-semibold: 600;
$font-weight-bold: 700;

// Отступы
$spacing-xs: 0.25rem;   // 4px
$spacing-sm: 0.5rem;    // 8px
$spacing-md: 1rem;      // 16px
$spacing-lg: 1.5rem;    // 24px
$spacing-xl: 2rem;      // 32px
$spacing-2xl: 3rem;     // 48px

// Границы
$border-radius-sm: 0.25rem;
$border-radius: 0.375rem;
$border-radius-lg: 0.5rem;
$border-radius-xl: 0.75rem;
$border-radius-full: 9999px;

// Тени
$shadow-sm: 0 1px 2px 0 rgb(0 0 0 / 0.05);
$shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1);
$shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);
$shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1);
$shadow-xl: 0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1);

// Breakpoints
$breakpoint-sm: 640px;
$breakpoint-md: 768px;
$breakpoint-lg: 1024px;
$breakpoint-xl: 1280px;
$breakpoint-2xl: 1536px;

// Z-index
$z-dropdown: 1000;
$z-sticky: 1020;
$z-fixed: 1030;
$z-modal-backdrop: 1040;
$z-modal: 1050;
$z-popover: 1060;
$z-tooltip: 1070;
```

---

### _mixins.scss

```scss
// Адаптивные миксины
@mixin mobile {
  @media (max-width: #{$breakpoint-sm - 1px}) {
    @content;
  }
}

@mixin tablet {
  @media (min-width: $breakpoint-sm) and (max-width: #{$breakpoint-lg - 1px}) {
    @content;
  }
}

@mixin desktop {
  @media (min-width: $breakpoint-lg) {
    @content;
  }
}

@mixin tablet-up {
  @media (min-width: $breakpoint-sm) {
    @content;
  }
}

@mixin desktop-up {
  @media (min-width: $breakpoint-lg) {
    @content;
  }
}

// Утилитарные миксины
@mixin flex-center {
  display: flex;
  align-items: center;
  justify-content: center;
}

@mixin flex-between {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

@mixin truncate {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

@mixin line-clamp($lines) {
  display: -webkit-box;
  -webkit-line-clamp: $lines;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

// Фокус
@mixin focus-ring {
  &:focus-visible {
    outline: 2px solid $primary;
    outline-offset: 2px;
  }
}

// Анимации
@mixin fade-in {
  animation: fadeIn 0.2s ease-in-out;
}

@mixin slide-up {
  animation: slideUp 0.2s ease-in-out;
}
```

---

### styles.scss

```scss
// Импорт переменных и миксинов
@use 'variables' as *;
@use 'mixins' as *;
@use 'reset';
@use 'typography';
@use 'utilities';
@use 'animations';
@use 'grid';

// Глобальные стили
*,
*::before,
*::after {
  box-sizing: border-box;
}

html {
  font-size: 16px;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

body {
  margin: 0;
  font-family: $font-family;
  font-size: $font-size-base;
  line-height: 1.5;
  color: $gray-800;
  background-color: $gray-50;
}

// Стили для скроллбара
::-webkit-scrollbar {
  width: 8px;
  height: 8px;
}

::-webkit-scrollbar-track {
  background: $gray-100;
}

::-webkit-scrollbar-thumb {
  background: $gray-300;
  border-radius: $border-radius-full;

  &:hover {
    background: $gray-400;
  }
}

// Глобальные классы
.container {
  width: 100%;
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 $spacing-md;
}

.visually-hidden {
  @include visually-hidden;
}
```

---

## Конфигурация маршрутизации

### app.routes.ts

```typescript
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'student',
    loadChildren: () => import('./features/student/student.routes').then(m => m.STUDENT_ROUTES)
  },
  {
    path: 'teacher',
    loadChildren: () => import('./features/teacher/teacher.routes').then(m => m.TEACHER_ROUTES)
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES)
  },
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  {
    path: '**',
    loadComponent: () => import('./shared/pages/not-found.page').then(m => m.NotFoundPage)
  }
];
```

---

## Конфигурация приложения

### app.config.ts

```typescript
import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding, withViewTransitions } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideStore } from '@ngrx/store';
import { routes } from './app.routes';
import { environment } from './environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(
      routes,
      withComponentInputBinding(),
      withViewTransitions()
    ),
    provideHttpClient(
      withInterceptors([
        authInterceptor,
        errorInterceptor,
        loadingInterceptor
      ])
    ),
    provideAnimations(),
    { provide: 'ENVIRONMENT', useValue: environment }
  ]
};
```

---

## Конфигурация Interceptors

### auth.interceptor.ts

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const token = tokenService.accessToken;
  
  if (token && !req.url.includes('/auth/login')) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  
  return next(req);
};
```

### error.interceptor.ts

```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const tokenService = inject(TokenService);
  const toastService = inject(ToastService);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        tokenService.clearTokens();
        router.navigate(['/auth/login']);
      } else if (error.status === 403) {
        router.navigate(['/forbidden']);
      } else if (error.status === 0) {
        toastService.error('Нет подключения к серверу');
      } else if (error.status >= 500) {
        toastService.error('Ошибка сервера');
      }
      
      return throwError(() => error);
    })
  );
};
```

---

## Конфигурация Docker

### Dockerfile

```dockerfile
# Stage 1: Build
FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# Stage 2: Serve
FROM nginx:alpine

COPY --from=build /app/dist/testsdelivery-web/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### nginx.conf

```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api/ {
        proxy_pass http://host.docker.internal:8080/api/;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

---

## Конфигурация CI/CD

### .github/workflows/ci.yml

```yaml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Use Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Lint
        run: npm run lint
      
      - name: Build
        run: npm run build
      
      - name: Test
        run: npm run test -- --watch=false
      
      - name: Build Docker
        run: docker build -t testsdelivery-web:${{ github.sha }} .
```
