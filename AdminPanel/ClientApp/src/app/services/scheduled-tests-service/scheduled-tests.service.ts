import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { scheduledTestsList as scheduledTestsListUrl } from 'src/app/models/endpoints';
import { ListFilter } from 'src/app/models/filters';
import { ScheduledTestsListModel } from 'src/app/models/scheduled-tests';

@Injectable({
  providedIn: 'root'
})
export class ScheduledTestsService {

  public constructor(private _http: HttpClient) { }

  public getScheduledTests(listFilter: ListFilter): Observable<ScheduledTestsListModel> {
    return this._http.post<ScheduledTestsListModel>(scheduledTestsListUrl, listFilter);
  }
}
