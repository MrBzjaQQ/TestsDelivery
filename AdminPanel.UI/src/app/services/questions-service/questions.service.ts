import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { questionsList, singleChoiceQuestion, multipleChoiceQuestion, essay, questionsInSubjectsList } from 'src/app/models/endpoints';
import { ListFilter, QuestionsInSubjectListFilterModel } from 'src/app/models/filters';
import { EssayCreateModel, EssayEditModel, EssayReadModel, McqCreateModel, McqEditModel, McqReadModel, QuestionsListModel, ScqCreateModel, ScqEditModel, ScqReadModel, SubjectWithQuestionsModel } from 'src/app/models/questions';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService {

  constructor(private _http: HttpClient) { }

  public getQuestionsForSubject(filter: QuestionsInSubjectListFilterModel) : Observable<QuestionsListModel> {
    return this._http.post<QuestionsListModel>(questionsList, filter);
  }

  public getQuestionsInSubjects(filter: ListFilter) : Observable<SubjectWithQuestionsModel[]> {
    return this._http.post<SubjectWithQuestionsModel[]>(questionsInSubjectsList, filter);
  }

  public createSingleChoice(question: ScqCreateModel) : Observable<ScqReadModel> {
    return this._http.post<ScqReadModel>(singleChoiceQuestion, question);
  }

  public createMultipleChoice(question: McqCreateModel) : Observable<McqReadModel> {
    return this._http.post<McqReadModel>(multipleChoiceQuestion, question);
  }

  public createEssay(question: EssayCreateModel) : Observable<EssayReadModel> {
    return this._http.post<EssayReadModel>(essay, question);
  }

  public getSingleChoice(id: number) : Observable<ScqReadModel> {
    return this._http.get<ScqReadModel>(`${singleChoiceQuestion}${id}`);
  }

  public getMultipleChoice(id: number) : Observable<McqReadModel> {
    return this._http.get<McqReadModel>(`${multipleChoiceQuestion}${id}`);
  }

  public getEssay(id: number) : Observable<EssayReadModel> {
    return this._http.get<EssayReadModel>(`${essay}${id}`);
  }

  public editSingleChoice(question: ScqEditModel) : Observable<any> {
    return this._http.put<any>(singleChoiceQuestion, question);
  }

  public editMultipleChoice(question: McqEditModel) : Observable<any> {
    return this._http.put<any>(multipleChoiceQuestion, question);
  }

  public editEssay(question: EssayEditModel) : Observable<any> {
    return this._http.put<any>(essay, question);
  }

}
