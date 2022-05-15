import { debounce } from 'lodash';
import { Component, OnInit } from '@angular/core';
import { MatSelectionListChange } from '@angular/material/list';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { ListFilter } from 'src/app/models/filters';
import { EssayReadModel, McqReadModel, QuestionType, ScqCreateModel, ScqReadModel, ShortQuestionModel } from 'src/app/models/questions';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';
import { EssayModel, McqModel, ScqModel } from 'src/app/models/components/questions-wizard';
import { ComponentType } from 'src/app/models/components';

// TODO: CRITICAL - USE FORM
// REWRITE COMPLETELY
@Component({
  selector: 'app-manage-questions',
  templateUrl: './manage-questions.component.html',
  styleUrls: ['./manage-questions.component.scss']
})
export class ManageQuestionsComponent implements OnInit {

  private _totalCount = 0;
  private _questionsList: ShortQuestionModel[] = [];
  private _componentType: ComponentType = 'create';
  public _selectedScq: ScqReadModel = {
    id: 0,
    name: '',
    text: '',
    answerOptions: [],
    subject: {
      id: 0,
      name: ''
    }
  };

  private _selectedMcq: McqReadModel = {
    id: 0,
    name: '',
    text: '',
    answerOptions: [],
    subject: {
      id: 0,
      name: ''
    }
  };

  private _selectedEssay: EssayReadModel = {
    id: 0,
    name: '',
    text: '',
    subject: {
      id: 0,
      name: ''
    }
  };

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

  public get componentType(): ComponentType {
    return this._componentType;
  }

  // TODO: get rid of implicit cast ScqReadModel -> ScqModel
  public get selectedScq(): ScqModel {
    return this._selectedScq;
  }

  // TODO: get rid of implicit cast McqReadModel -> McqModel
  public get selectedMcq(): McqModel {
    return this._selectedMcq;
  }

  // TODO: get rid of implicit cast EssayReadModel -> EssayModel
  public get selectedEssay(): EssayModel {
    return this._selectedEssay;
  }

  public onPageEventChanged(value: PageEvent) : void {
    this._questionsListFilter.take = value.pageSize;
    this._questionsListFilter.skip = value.pageSize * value.pageIndex;
    this._loadQuestionsForSubject();
  }

  public onQuestionSelected(event: MatSelectionListChange) {
    this._selectedQuestion = event.source.selectedOptions.selected[0]?.value as ShortQuestionModel;
    switch(this._selectedQuestion.type) {
      case QuestionType.SingleChoice: {
        this._questionsService.getSingleChoice(this._selectedQuestion.id)
        .subscribe(response => {
          this._selectedScq = response;
          this._componentType = 'edit';
        });
        break;
      }
      case QuestionType.MultipleChoice: {
        this._questionsService.getMultipleChoice(this._selectedQuestion.id)
        .subscribe(response => {
          this._selectedMcq = response;
          this._componentType = 'edit';
        });
        break;
      }
      case QuestionType.Essay: {
        this._questionsService.getEssay(this._selectedQuestion.id)
        .subscribe(response => {
          this._selectedEssay = response;
          this._componentType = 'edit';
        });
        break;
      }
      default:
        break;
    }
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
      this._selectedScq = result;
      this._setShortModel({
        id: result.id,
        name: result.name,
        type: QuestionType.SingleChoice
      });
    });
  }

  public onMultipleChoiceSaved(model: McqModel) : void {
    this._questionsService.createMultipleChoice({
      subjectId: this._subjectId,
      answerOptions: model.answerOptions,
      name: model.name,
      text: model.text
    }).subscribe(result => {
      this._selectedMcq = result;
      this._setShortModel({
        id: result.id,
        name: result.name,
        type: QuestionType.MultipleChoice
      });
    });
  }

  // TODO: it creates new essay instead of editing it
  public onEssaySaved(model: EssayModel) : void {
    this._questionsService.createEssay({
      subjectId: this._subjectId,
      name: model.name,
      text: model.text
    }).subscribe(result => {
      this._selectedEssay = result;
      this._setShortModel({
        id: result.id,
        name: result.name,
        type: QuestionType.Essay
      });
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

  private _setShortModel(shortModel: ShortQuestionModel) : void {
    switch (this._componentType) {
      case 'create': {
        this._componentType = 'edit';
        this._selectedQuestion = shortModel;
        this._questionsList.push(shortModel);
        break;
      }
      case 'edit': {
        const [ question ] = this._questionsList.filter(x => x.id == this._selectedQuestion.id);
        const index = this._questionsList.indexOf(question);
        this._questionsList[index] = shortModel;
        break;
      }
      default:
        throw new Error('Unknown component type');
    }

  }

}
