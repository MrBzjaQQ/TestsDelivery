import { NestedTreeControl } from '@angular/cdk/tree';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ComponentType } from 'src/app/models/components';
import { QuestionsTreeNode } from 'src/app/models/components/manage-test';
import { QuestionInSubjectModel, SubjectWithQuestionsModel } from 'src/app/models/questions';
import { TestCreateModel, TestEditModel } from 'src/app/models/tests';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';
import { TestsService } from 'src/app/services/tests-service/tests.service';

@Component({
  selector: 'app-manage-test',
  templateUrl: './manage-test.component.html',
  styleUrls: ['./manage-test.component.scss']
})
export class ManageTestComponent implements OnInit {

  private _name: string = '';
  private _subjectsWithQuestions: SubjectWithQuestionsModel[] = [];
  private _searchText: string = '';
  private _selectedSubjectsWithQuestions: QuestionsTreeNode[] = [];

  constructor(
    private _testsService: TestsService,
    private _route: ActivatedRoute,
    private _questionsService: QuestionsService,
    private _cdr: ChangeDetectorRef,
    private _location: Location) { }

  ngOnInit(): void {
    this.getQuestionsTree();
  }

  public sourceTreeControl = new NestedTreeControl<QuestionsTreeNode>(node => node.children);
  public sourceDataSource = new MatTreeNestedDataSource<QuestionsTreeNode>();

  public destinationTreeControl = new NestedTreeControl<QuestionsTreeNode>(node => node.children);
  public destinationDataSource = new MatTreeNestedDataSource<QuestionsTreeNode>();

  public getQuestionsTree(): void {
    this._questionsService.getQuestionsInSubjects({
      searchText: this._searchText
    }).subscribe(response => {
      this._subjectsWithQuestions = [...response];
      this.sourceDataSource.data = this._mapToQuestionsTreeNode([...response]);
    });
  }

  public get subjectsWithQuesitons(): SubjectWithQuestionsModel[] {
    return this._subjectsWithQuestions;
  }

  public get name() : string {
    return this._name;
  }

  public hasChild = (_: number, node: QuestionsTreeNode) => !!node.children && node.children.length > 0;

  public add(node: QuestionsTreeNode): void {
    this._moveNode(
      this.sourceDataSource,
      this.destinationDataSource,
      node
    );
  }

  public remove(node: QuestionsTreeNode): void {
    this._moveNode(
      this.destinationDataSource,
      this.sourceDataSource,
      node
    );
  }

  public onTestNameChanged(event: Event) {
    const input = event.target as HTMLInputElement;
    this._name = input.value;
  }

  public saveChanges() : void {
    const createModel = this._getCreateTestData();
    this._testsService.createTest(createModel).subscribe(x => {
      this._location.replaceState(`/tests/${x.id}`);
    });
  }

  public discardChanges() : void {
    this._name = '';
    this.sourceDataSource.data = this._mapToQuestionsTreeNode(this._subjectsWithQuestions);
    this.destinationDataSource.data = [];
  }

  private get _componentType(): ComponentType {
    return Number.isFinite(this._testId) ? 'edit' : 'create';
  }

  private get _testId(): number | null {
    return Number(this._route.snapshot.paramMap.get('subjectId')) || null;
  }

  private _mapToQuestionsTreeNode(subjectsWithQuestions: SubjectWithQuestionsModel[]): QuestionsTreeNode[] {
    return subjectsWithQuestions.map((node: SubjectWithQuestionsModel): QuestionsTreeNode => {
      return {
        name: node.name,
        level: 0,
        id: node.id,
        children: !!node.questions ? node.questions.map((question: QuestionInSubjectModel): QuestionsTreeNode => {
          return {
            level: 1,
            name: question.name,
            children: [],
            id: question.id,
          };
        }) : []
      };
    });
  }

  private _moveNode(source: MatTreeNestedDataSource<QuestionsTreeNode>, dest: MatTreeNestedDataSource<QuestionsTreeNode>, node: QuestionsTreeNode) : void {
    if (node.level === 0) {
      const index = source.data.findIndex(x => x.id == node.id);
      source.data.splice(index, 1).sort((prev, curr) => prev.id - curr.id);
      source.data = JSON.parse(JSON.stringify(source.data));

      dest.data.push(node);
      dest.data.sort((prev, curr) => prev.id - curr.id);
      dest.data = JSON.parse(JSON.stringify(dest.data));

      return;
    }

    const parentSourceIndex = source.data.findIndex(x => x.children.findIndex(x => x.id === node.id) !== -1);
    const parentSource = source.data[parentSourceIndex];
    parentSource.children = parentSource.children.filter(x => x.id !== node.id);
    parentSource.children.sort((prev, curr) => prev.id - curr.id);
    parentSource.children = [...parentSource.children];
    if (parentSource.children.length === 0) {
      source.data.splice(parentSourceIndex, 1);
    } else {
      source.data[parentSourceIndex] = {
        ...parentSource
      };
    }

    const parentDestIndex = dest.data.findIndex(x => x.id === parentSource.id);
    let parentDest : QuestionsTreeNode;
    if (parentDestIndex === -1) {
      parentDest = {
        id: parentSource.id,
        level: parentSource.level,
        name: parentSource.name,
        children: []
      };

      dest.data.push(parentDest);
    } else {
      parentDest = dest.data[parentDestIndex];
    }

    parentDest.children.push(node);
    parentDest.children.sort((prev, curr) => prev.id - curr.id);
    parentDest.children = [...parentDest.children];

    source.data = JSON.parse(JSON.stringify(source.data));
    dest.data = JSON.parse(JSON.stringify(dest.data));
  }

  private _getCreateTestData() : TestCreateModel {
    const questions : QuestionsTreeNode[] = [];
    this.destinationDataSource.data.forEach(x => questions.push(...x.children));

    return {
      name: this._name,
      questionIds: questions.map(x => x.id)
    };
  }

  private _saveCreateModel(model: TestCreateModel) {

  }

  private _saveEditModel(model: TestEditModel) {
    
  }
}
