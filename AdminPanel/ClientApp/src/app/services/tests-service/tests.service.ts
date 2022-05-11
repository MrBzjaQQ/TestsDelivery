import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ListFilter } from 'src/app/models/filters';
import { TestsList } from 'src/app/models/tests';
import { testsList as testsListUrl } from 'src/app/models/endpoints';

@Injectable({
  providedIn: 'root'
})
export class TestsService {

  constructor(private _http: HttpClient) { }

  public getTests(filter: ListFilter) : Observable<TestsList> {
    return this._http.post<TestsList>(testsListUrl, filter);
  }
}
