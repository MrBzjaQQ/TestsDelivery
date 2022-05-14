import { debounce } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { MatSelectionListChange } from '@angular/material/list';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { ListFilter } from 'src/app/models/filters';
import { QuestionType, ShortQuestionModel } from 'src/app/models/questions';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';
import { EssayModel, McqModel, ScqModel } from 'src/app/models/components/questions-wizard';

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
  private _selectedQuestion: ShortQuestionModel = {
    id: 0,
    name: '',
    type: QuestionType.SingleChoice
  };

  public readonly pageSizeOptions = [ 25, 50, 100 ];

  constructor(
    private _route: ActivatedRoute,
    private _questionsService: QuestionsService) {}

  public ngOnInit(): void {
    this._loadQuestionsForSubject();
    this._selectedQuestion = this._questionsList[0];
  }

  public get questionsList() : ShortQuestionModel[] {
    return this._questionsList;
  }

  public get totalCount() : number {
    return this._totalCount;
  }

  public get selectedQuestion() : ShortQuestionModel {
    return this._selectedQuestion;
  }

  public get type() : QuestionType {
    return this.selectedQuestion.type;
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._questionsListFilter.take = value.pageSize;
    this._questionsListFilter.skip = value.pageSize * value.pageIndex;
    this._loadQuestionsForSubject();
  }

  public onQuestionSelected(event: MatSelectionListChange) {
    this._selectedQuestion = event.source.selectedOptions.selected[0]?.value as ShortQuestionModel;
  }

  public onTextChanged = debounce((text: string) : void => {
    this._questionsListFilter.searchText = text;
    this._loadQuestionsForSubject();
  }, 500);

  public onSingleChoiceSaved(model: ScqModel) : void {
    this._questionsService.createSingleChoice({
      subjectId: this._subjectId,
      answerOptions: model.answerOptions,
      name: model.name,
      text: model.text
    }).subscribe(result => {
      // TODO:
    });
  }

  public onMultipleChoiceSaved(model: McqModel) : void {
    this._questionsService.createMultipleChoice({
      subjectId: this._subjectId,
      answerOptions: model.answerOptions,
      name: model.name,
      text: model.text
    }).subscribe(result => {
      // TODO:
    });
  }

  public onEssaySaved(model: EssayModel) : void {
    this._questionsService.createEssay({
      subjectId: this._subjectId,
      name: model.name,
      text: model.text
    }).subscribe(result => {
      // TODO:
    });
  }

  private get _subjectId() : number {
    return Number(this._route.snapshot.paramMap.get('subjectId'));
  }

  private _loadQuestionsForSubject() : void {
    this._questionsService.getQuestionsForSubject({
      ...this._questionsListFilter,
      subjectId: this._subjectId
    }).subscribe(response => {
      this._questionsList = [...response.questions];
      this._totalCount = response.totalCount;
    });
  }

}
