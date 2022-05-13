import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { questionsList, singleChoiceQuestion } from 'src/app/models/endpoints';
import { QuestionsInSubjectListFilterModel } from 'src/app/models/filters';
import { QuestionsListModel, ScqCreateModel, ScqReadModel } from 'src/app/models/questions';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  constructor(private _http: HttpClient) { }

  public getQuestionsForSubject(filter: QuestionsInSubjectListFilterModel) : Observable<QuestionsListModel> {
    return this._http.post<QuestionsListModel>(questionsList, filter);
  }

  public createSingleChoice(question: ScqCreateModel) : Observable<ScqReadModel> {
    return this._http.post<ScqReadModel>(singleChoiceQuestion, question);
  }

  public createMultipleChoice(question: ScqCreateModel) : Observable<ScqReadModel> {
    return this._http.post<ScqReadModel>(singleChoiceQuestion, question);
  }

  public createEssay() {
    
  }

}
