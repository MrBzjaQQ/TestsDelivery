import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ComponentType } from 'src/app/models/components';
import { QuestionsTreeNode } from 'src/app/models/components/manage-test';
import { QuestionInSubjectModel, SubjectWithQuestionsModel } from 'src/app/models/questions';
import { TestCreateModel, TestEditModel, TestReadModel } from 'src/app/models/tests';
import { QuestionsService } from 'src/app/services/questions-service/questions.service';
import { TestsService } from 'src/app/services/tests-service/tests.service';
import { cloneDeep, xorBy } from 'lodash';

// TODO: There are a lot of .find in forEach.
// Refactor it because of performance loss.
// TODO: optimize using lodash
@Component({
  selector: 'app-manage-test',
  templateUrl: './manage-test.component.html',
  styleUrls: ['./manage-test.component.scss']
})
export class ManageTestComponent implements OnInit {

  private _name: string = '';

  private _defaultName: string = '';
  private _destinationTree: QuestionsTreeNode[] = [];
  private _sourceTree: QuestionsTreeNode[] = [];

  constructor(
    private _testsService: TestsService,
    private _route: ActivatedRoute,
    private _questionsService: QuestionsService,
    private _location: Location) { }

  ngOnInit(): void {
    this._initCreateDialog();
  }

  public sourceTreeControl = new NestedTreeControl<QuestionsTreeNode>(node => node.children);
  public sourceDataSource = new MatTreeNestedDataSource<QuestionsTreeNode>();

  public destinationTreeControl = new NestedTreeControl<QuestionsTreeNode>(node => node.children);
  public destinationDataSource = new MatTreeNestedDataSource<QuestionsTreeNode>();

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
    switch(this._componentType) {
      case 'create': {
        this._saveCreateModel();
        break;
      }
      case 'edit': {
        this._saveEditModel();
        break;
      }
      default:
        throw new Error('Unknown component type.');
    }
  }

  public discardChanges() : void {
    this._name = this._defaultName;
    this.sourceDataSource.data = cloneDeep(this._sourceTree);
    this.destinationDataSource.data = cloneDeep(this._destinationTree);
  }

  private get _componentType(): ComponentType {
    return Number.isFinite(this._testId) ? 'edit' : 'create';
  }

  private get _testId(): number | null {
    return Number(this._route.snapshot.paramMap.get('id')) || null;
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

  private _getEditTestData() : TestEditModel {
    const questions : QuestionsTreeNode[] = [];
    this.destinationDataSource.data.forEach(x => questions.push(...x.children));
    const id = this._testId;
    if (!id)
      throw new Error('Id should be set in location state for editing test');

    return {
      id,
      name: this._name,
      questionIds: questions.map(x => x.id)
    };
  }

  private _saveCreateModel(): void {
    const createModel = this._getCreateTestData();
    this._testsService.createTest(createModel).subscribe(x => {
      this._location.replaceState(`/tests/${x.id}`);
    });
  }

  private _saveEditModel(): void {
    const editModel = this._getEditTestData();
    this._testsService.editTest(editModel).subscribe(x => {
      this._location.replaceState(`/tests/${x.id}`);
    });
  }
  
  private _initCreateDialog(): void {
    this._questionsService.getQuestionsInSubjects({}).subscribe(response => {
      this.sourceDataSource.data = this._mapToQuestionsTreeNode([...response]);
      this._sourceTree = cloneDeep(this.sourceDataSource.data);

      if (this._componentType === 'edit')
        this._initEditDialog();
    });
  }

  private _initEditDialog(): void {
    const id = this._testId;
    if (!id)
      throw new Error('Id is required for getting test details.');
    this._testsService.getTestDetails(id)
      .subscribe((response) => {
        this._defaultName = response.name;
        this._name = response.name;
        this._initEditTreeInternal(response);
        this._filterSourceTree();
        this._sourceTree = cloneDeep(this.sourceDataSource.data);
        this._destinationTree = cloneDeep(this.destinationDataSource.data);
      });
  }

  private _initEditTreeInternal(model: TestReadModel): void {
    const tree: QuestionsTreeNode[] = [];
    model.questions.forEach(question => {
      // TODO: .find
      let subject = tree.find(subject => subject.id === question.subject.id);
      if (!subject) {
        subject = {
          id: question.subject.id,
          name: question.subject.name,
          level: 0,
          children: []
        };

        tree.push(subject);
      }

      subject.children.push({
        id: question.id,
        name: question.name,
        level: 1,
        children: []
      });
    });

    this.destinationDataSource.data = tree;
  }

  private _filterSourceTree(): void {
    this.destinationDataSource.data.forEach(subject => {
      const sourceSubject = this.sourceDataSource.data.find(x => x.id === subject.id);
      if (!sourceSubject)
        throw new Error('Source subject should be in tree');
      
      sourceSubject.children = sourceSubject.children.filter(x => subject.children.findIndex(y => y.id === x.id) === -1)
    });

    this.sourceDataSource.data = cloneDeep(this.sourceDataSource.data);
  }
}
