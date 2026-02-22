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
