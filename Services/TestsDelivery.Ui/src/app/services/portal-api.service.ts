import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, StudentProfile, AvailableTests, TestQuestions, StartTestResponse, SubmitTestRequest, SubmitTestResponse, TestResults, GroupAnalytics } from '../models';

@Injectable({
  providedIn: 'root'
})
export class PortalApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/v1/portal';

  getStudentProfile(): Observable<ApiResponse<StudentProfile>> {
    return this.http.get<ApiResponse<StudentProfile>>(`${this.baseUrl}/students/profile`);
  }

  getAvailableTests(groupId?: string, status?: string): Observable<ApiResponse<AvailableTests>> {
    let params = new HttpParams();
    if (groupId) {
      params = params.set('groupId', groupId);
    }
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<ApiResponse<AvailableTests>>(`${this.baseUrl}/tests/available`, { params });
  }

  startTest(testId: string): Observable<ApiResponse<StartTestResponse>> {
    return this.http.post<ApiResponse<StartTestResponse>>(`${this.baseUrl}/tests/${testId}/start`, {});
  }

  getTestQuestions(testId: string): Observable<ApiResponse<TestQuestions>> {
    return this.http.get<ApiResponse<TestQuestions>>(`${this.baseUrl}/tests/${testId}/questions`);
  }

  submitTest(testId: string, request: SubmitTestRequest): Observable<ApiResponse<SubmitTestResponse>> {
    return this.http.post<ApiResponse<SubmitTestResponse>>(`${this.baseUrl}/tests/${testId}/submit`, request);
  }

  getTestResults(testId: string): Observable<ApiResponse<TestResults>> {
    return this.http.get<ApiResponse<TestResults>>(`${this.baseUrl}/tests/${testId}/results`);
  }

  getGroupAnalytics(groupId: string): Observable<ApiResponse<GroupAnalytics>> {
    return this.http.get<ApiResponse<GroupAnalytics>>(`${this.baseUrl}/groups/${groupId}/analytics`);
  }
}
