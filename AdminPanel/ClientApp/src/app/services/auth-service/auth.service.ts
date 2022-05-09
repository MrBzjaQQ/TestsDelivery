import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequestModel, LoginSucceedResponseModel } from '../../models/user/login';
import { RegisterRequestModel, RegisterSucceedResponseModel } from '../../models/user/register';
import { login, register } from '../../models/endpoints';
import { LocalStorageService } from '../local-storage-service/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _token: string = '';
  private _userName: string = '';

  constructor(
    private http: HttpClient,
    private localStorageService : LocalStorageService) { }

  public get token(): string {
    return this._token;
  }

  public get userName(): string {
    return this._userName;
  }

  public get isAuthenticated() : boolean {
    return !!this._userName && !!this._token;
  }

  public LogIn(requestModel: LoginRequestModel): void {
    this.http.post<LoginSucceedResponseModel>(login, requestModel)
      .subscribe(response => {
        this._token = response.accessToken;
        this._userName = response.userName;

        this.localStorageService.setAuthenticationInfo(response);
      });
  }

  public Register(requestModel: RegisterRequestModel) {
    this.http.post<RegisterSucceedResponseModel>(register, requestModel)
      .subscribe(response => {
        this.LogIn({
          userName: requestModel.userName,
          password: requestModel.password
        });
      });
  }

}
