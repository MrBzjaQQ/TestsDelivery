# Angular 21 UI - Запуск

## Требования

### Программное обеспечение

| Инструмент | Версия | Назначение |
|------------|--------|------------|
| Node.js | 20.x LTS | Среда выполнения |
| npm | 10.x | Менеджер пакетов |
| Git | 2.x | Контроль версий |

### Проверка установки

```bash
# Проверка Node.js
node --version
# Должно вывести: v20.x.x

# Проверка npm
npm --version
# Должно вывести: 10.x.x
```

---

## Установка и запуск

### 1. Клонирование репозитория

```bash
git clone https://github.com/your-org/TestsDelivery.git
cd TestsDelivery/src/TestsDelivery.Web
```

### 2. Установка зависимостей

```bash
npm install
```

### 3. Настройка окружения

```bash
# Копирование файла окружения
cp src/environments/environment.ts.sample src/environments/environment.ts

# Редактирование настроек
nano src/environments/environment.ts
```

**environment.ts:**
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:8080/api/v1',
  fileServiceUrl: 'http://localhost:8085',
  identityUrl: 'http://localhost:8081',
  version: '1.0.0-dev'
};
```

### 4. Запуск dev-сервера

```bash
# Стандартный запуск
npm start

# или
ng serve

# С указанием порта
ng serve --port 4200

# С hot reload
ng serve --hmr
```

### 5. Доступ к приложению

```
http://localhost:4200
```

---

## Режимы запуска

### Development

```bash
npm start
# или
ng serve
```

Особенности:
- Hot Module Replacement (HMR)
- Source maps
- Детальные сообщения об ошибках
- Локальный API

### Production

```bash
# Сборка
npm run build

# Запуск production сборки
npm run serve:prod
```

Особенности:
- Минификация
- Tree shaking
- AOT компиляция
- Bundle optimization

### Storybook

```bash
# Запуск Storybook
npm run storybook

# Сборка Storybook
npm run build-storybook
```

---

## Скрипты npm

### Доступные скрипты

| Скрипт | Команда | Описание |
|--------|---------|----------|
| start | `ng serve` | Запуск dev-сервера |
| build | `ng build` | Сборка проекта |
| test | `ng test` | Запуск unit-тестов |
| lint | `ng lint` | Проверка кода линтером |
| e2e | `ng e2e` | End-to-end тесты |
| format | `npx prettier --write` | Форматирование кода |

### Дополнительные команды

```bash
# Генерация компонента
ng generate component features/student/test-card
# или кратко
ng g c features/student/test-card

# Генерация сервиса
ng g s core/services/auth

# Генерация guard
ng g g core/guards/auth

# Генерация pipe
ng g p shared/pipes/date-format

# Генерация interface
ng g i core/models/user
```

---

## Docker запуск

### Dockerfile

```bash
# Сборка образа
docker build -t testsdelivery-web:latest .

# Запуск контейнера
docker run -p 4200:80 testsdelivery-web:latest
```

### Docker Compose

```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "4200:80"
    environment:
      - API_URL=http://localhost:8080/api/v1
    depends_on:
      - api

  api:
    image: testsdelivery-api:latest
    ports:
      - "8080:8080"
```

---

## Структура директорий для запуска

```
TestsDelivery.Web/
├── node_modules/          # Зависимости (устанавливаются автоматически)
├── src/
│   ├── app/
│   │   ├── core/          # Сервисы, guards, interceptors
│   │   ├── features/      # Страницы и компоненты
│   │   ├── shared/        # Общие компоненты
│   │   └── layout/        # Layout компоненты
│   ├── assets/            # Статические файлы
│   ├── environments/      # Конфигурация окружения
│   └── styles/            # Глобальные стили
├── angular.json           # Конфигурация Angular CLI
├── package.json
├── tsconfig.json
├── tsconfig.app.json
└── tsconfig.spec.json
```

---

## Устранение проблем

### Ошибка Node.js версии

```bash
# Использование nvm для切换 Node.js версии
nvm install 20
nvm use 20
```

### Ошибка порта

```bash
# Проверка занятого порта
lsof -i :4200

# Запуск на другом порту
ng serve --port 4201
```

### Ошибка зависимостей

```bash
# Очистка кэша и переустановка
rm -rf node_modules
npm cache clean --force
npm install
```

### Ошибки компиляции

```bash
# Проверка TypeScript
npx tsc --noEmit

# Обновление Angular CLI
npm install @angular/cli@latest
```

---

## Сборка для production

### Оптимизированная сборка

```bash
# Production сборка
npm run build

# Сборка с анализом размера бандлов
npm run build -- --stats-json
npx webpack-bundle-analyzer dist/stats.json
```

### Проверка production сборки локально

```bash
# Установка serve
npm install -g serve

# Запуск
serve -s dist/testsdelivery-web
```

---

## CI/CD интеграция

### GitHub Actions

```yaml
name: Build and Deploy

on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup Node.js
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
      
      - name: Upload artifacts
        uses: actions/upload-artifact@v4
        with:
          name: dist
          path: dist/
```

---

## Мониторинг

### Angular DevTools

1. Установить расширение Chrome DevTools
2. Открыть вкладку "Angular" в DevTools
3. Просматривать:
   - Component tree
   - Properties
   - Injectors
   - Change detection

### Webpack Bundle Analyzer

```bash
npm install -g webpack-bundle-analyzer
webpack-bundle-analyzer dist/testsdelivery-web/stats.json
```
