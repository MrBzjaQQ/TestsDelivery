import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CandiadteEditModel, CandidateCreateModel, CandidateReadModel, CandidatesList } from 'src/app/models/candidates';
import { candidate as candidateUrl, candidatesList as candidatesListUrl } from 'src/app/models/endpoints';
import { ListFilter } from 'src/app/models/filters';

@Injectable({
  providedIn: 'root'
})
export class CandidatesService {

  // TODO: use underscore for private fields
  constructor(private http: HttpClient) { }

  public getCandidates(filter: ListFilter) : Observable<CandidatesList> {
     return this.http.post<CandidatesList>(candidatesListUrl, filter);
  }

  public createCandidate(candidateModel: CandidateCreateModel) : Observable<CandidateReadModel> {
    return this.http.post<CandidateReadModel>(candidateUrl, candidateModel);
  }

  public editCandidate(candidateModel: CandiadteEditModel) : Observable<any> {
    return this.http.put<any>(candidateUrl, candidateModel);
  }
}
