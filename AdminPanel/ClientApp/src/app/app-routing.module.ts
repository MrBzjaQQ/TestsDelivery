import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidatesListComponent } from './components/candidates-list/candidates-list.component';
import { HomeComponent } from './components/home-component/home.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { ManageQuestionsComponent } from './components/manage-questions/manage-questions.component';
import { ManageTestComponent } from './components/manage-test/manage-test.component';
import { RegisterFormComponent } from './components/register-form/register-form.component';
import { ScheduledTestsListComponent } from './components/scheduled-tests-list/scheduled-tests-list.component';
import { SubjectsListComponent } from './components/subjects-list/subjects-list.component';
import { TestsListComponent } from './components/tests-list/tests-list.component';

// TODO: routing example https://angular.io/tutorial/toh-pt5#heroescomponent-hero-links
const routes: Routes = [
  { path: 'login', component: LoginFormComponent },
  { path: 'register', component: RegisterFormComponent },
  { path: 'candidates', component: CandidatesListComponent },
  { path: 'subjects', component: SubjectsListComponent },
  { path: 'tests', component: TestsListComponent },
  { path: 'scheduledTests', component: ScheduledTestsListComponent },
  // TODO: probably should be child of subjects with :id
  { path: 'subjects/:subjectId', component: ManageQuestionsComponent },
  // TODO: it should be /test/{id}
  { path: 'tests/new', component: ManageTestComponent },
  { path: 'tests/:id', component: ManageTestComponent },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
