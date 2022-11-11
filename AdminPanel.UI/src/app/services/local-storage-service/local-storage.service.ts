import { Injectable } from '@angular/core';
import { authenticationTokenKey } from 'src/app/models/local-storage';
import { AuthenticationInfo } from 'src/app/models/user/login';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  constructor() { }

  getAuthenticationInfo() : AuthenticationInfo | null {
    const infoStr = localStorage.getItem(authenticationTokenKey);
    if (infoStr)
      return JSON.parse(infoStr) as AuthenticationInfo;
    
    return null;
  }

  setAuthenticationInfo(authInfo: AuthenticationInfo) : void {
    const infoStr = JSON.stringify(authInfo);
    localStorage.setItem(authenticationTokenKey, infoStr);
  }

}
