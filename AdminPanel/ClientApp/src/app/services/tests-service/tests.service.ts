import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ListFilter } from 'src/app/models/filters';
import { TestCreateModel, TestEditModel, TestReadModel, TestsList } from 'src/app/models/tests';
import { test as testUrl, testsList as testsListUrl } from 'src/app/models/endpoints';

@Injectable({
  providedIn: 'root'
})
export class TestsService {

  constructor(private _http: HttpClient) { }

  public getTests(filter: ListFilter) : Observable<TestsList> {
    return this._http.post<TestsList>(testsListUrl, filter);
  }

  public getTestDetails(id: number) : Observable<TestReadModel> {
    return this._http.get<TestReadModel>(`${testUrl}${id}`);
  }

  public createTest(testModel: TestCreateModel) : Observable<TestReadModel> {
    return this._http.post<TestReadModel>(testUrl, testModel);
  }

  public editTest(testModel: TestEditModel) : Observable<any> {
    return this._http.put<any>(testUrl, testModel);
  }
}
