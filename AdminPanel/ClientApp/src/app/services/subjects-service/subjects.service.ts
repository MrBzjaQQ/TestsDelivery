import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ListFilter } from 'src/app/models/filters';
import { SubjectCreateModel, SubjectEditModel, SubjectReadModel, SubjectsListModel } from 'src/app/models/subjects';
import { subjects as subjectsUrl , subjectsList as subjectsListUrl } from 'src/app/models/endpoints';

@Injectable({
  providedIn: 'root'
})
export class SubjectsService {

  constructor(private http: HttpClient) { }

  public getSubjects(filter: ListFilter) : Observable<SubjectsListModel> {
    return this.http.post<SubjectsListModel>(subjectsListUrl, filter);
  }

  public createSubject(subject: SubjectCreateModel) : Observable<SubjectCreateModel> {
    return this.http.post<SubjectReadModel>(subjectsUrl, subject);
  }

  public editSubject(subject: SubjectEditModel) : Observable<any> {
    return this.http.put<any>(subjectsUrl, subject);
  }
}
