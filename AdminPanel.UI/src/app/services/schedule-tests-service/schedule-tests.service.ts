import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { schedule as scheduleUrl } from 'src/app/models/endpoints';
import { ScheduledTestCreateModel, ScheduledTestReadModel } from 'src/app/models/scheduled-tests';

@Injectable({
  providedIn: 'root'
})
export class ScheduleTestsService {

  public constructor(private _http: HttpClient) { }

  public scheduleTest(model: ScheduledTestCreateModel) : Observable<ScheduledTestReadModel> {
    return this._http.post<ScheduledTestReadModel>(scheduleUrl, model);
  }
}
