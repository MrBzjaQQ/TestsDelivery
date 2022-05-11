import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ListFilter, QuestionsInSubjectListFilterModel } from 'src/app/models/filters';
import { ShortQuestionModel } from 'src/app/models/questions';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';

@Component({
  selector: 'app-manage-questions',
  templateUrl: './manage-questions.component.html',
  styleUrls: ['./manage-questions.component.scss']
})
export class ManageQuestionsComponent implements OnInit {

  private _totalCount = 0;
  private _questionsList: ShortQuestionModel[] = [];
  private _questionsListFilter: ListFilter = {
    take: 25,
    skip: 0,
    searchText: undefined
  };

  constructor(
    private _route: ActivatedRoute,
    private _location: Location,
    private _questionsService: QuestionsService) {}

  public ngOnInit(): void {
    this.loadQuestionsForSubject();
  }

  public get questionsList() : ShortQuestionModel[] {
    return this._questionsList;
  }

  private get _subjectId() : number {
    return Number(this._route.snapshot.paramMap.get('id'));
  }

  private loadQuestionsForSubject() : void {
    this._questionsService.getQuestionsForSubject({
      ...this._questionsListFilter,
      subjectId: this._subjectId
    }).subscribe(response => {
      this._questionsList = [...response.questions];
      this._totalCount = response.totalCount;
    });
  }

}
