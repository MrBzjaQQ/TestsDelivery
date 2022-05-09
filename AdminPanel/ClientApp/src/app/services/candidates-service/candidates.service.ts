import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CandidatesList } from 'src/app/models/candidates';
import { candidatesList } from 'src/app/models/endpoints';
import { ListFilter } from 'src/app/models/filters';

@Injectable({
  providedIn: 'root'
})
export class CandidatesService {

  constructor(private http: HttpClient) { }

  public getCandidates(filter: ListFilter) : Observable<CandidatesList> {
     return this.http.post<CandidatesList>(candidatesList, filter);
  }
}
