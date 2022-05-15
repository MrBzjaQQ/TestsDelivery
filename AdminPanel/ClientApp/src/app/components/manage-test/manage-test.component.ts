import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { ComponentType } from 'src/app/models/components';
import { ListFilter } from 'src/app/models/filters';
import { SubjectWithQuestionsModel } from 'src/app/models/questions';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';
import { TestsService } from 'src/app/services/tests-service/tests.service';

@Component({
  selector: 'app-manage-test',
  templateUrl: './manage-test.component.html',
  styleUrls: ['./manage-test.component.scss']
})
export class ManageTestComponent implements OnInit {

  private _componentType: ComponentType = 'create';
  private _name: string = '';
  private _subjectsWithQuestions: SubjectWithQuestionsModel[] = [];
  private _searchText: string = '';

  constructor(
    private _testsService: TestsService,
    private _questionsService: QuestionsService) { }

  ngOnInit(): void {
    this.getQuestionsTree();
  }

  // TODO: tree control
  public questionsTreeControl = new FlatTreeControl<SubjectWithQuestionsModel>(
    node => 0,
    node => !!node.questions);

  public getQuestionsTree(): void {
    this._questionsService.getQuestionsInSubjects({
      searchText: this._searchText
    }).subscribe(response => {
      this._subjectsWithQuestions = [...response];
    });
  }

  public get subjectsWithQuesitons() : SubjectWithQuestionsModel[] {
    return this._subjectsWithQuestions;
  }

}
