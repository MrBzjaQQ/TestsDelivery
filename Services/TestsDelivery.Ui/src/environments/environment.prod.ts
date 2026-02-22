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
