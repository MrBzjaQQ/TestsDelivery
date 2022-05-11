import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { questionsList } from 'src/app/models/endpoints';
import { QuestionsInSubjectListFilterModel } from 'src/app/models/filters';
import { QuestionsListModel } from 'src/app/models/questions';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  constructor(private _http: HttpClient) { }

  public getQuestionsForSubject(filter: QuestionsInSubjectListFilterModel) : Observable<QuestionsListModel> {
    return this._http.post<QuestionsListModel>(questionsList, filter);
  }
}
