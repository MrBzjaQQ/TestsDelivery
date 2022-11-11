import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { portalInstances as portalInstancesUrl } from 'src/app/models/endpoints';

@Injectable({
  providedIn: 'root'
})
export class TestPortalInstancesService {

  constructor(private _http: HttpClient) { }

  public getInstances(): Observable<string[]> {
    return this._http.get<string[]>(portalInstancesUrl);
  }
}
