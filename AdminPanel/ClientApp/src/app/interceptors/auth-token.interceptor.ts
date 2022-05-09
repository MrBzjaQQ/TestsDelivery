import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocalStorageService } from '../services/local-storage-service/local-storage.service';

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {

  constructor(private localStorageService: LocalStorageService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const info = this.localStorageService.getAuthenticationInfo();
    if (info)
      return next.handle(request.clone({
        setHeaders: {
          'Authorization': `Bearer ${info.accessToken}`
        }
      }));
    return next.handle(request);
  }
}
